using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using Swim_Feedback.Data;
using Swim_Feedback.Data.Migrations;
using Swim_Feedback.Pages;
using Swim_Feedback.Services.PageDataService;
using Swim_Feedback.Services;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Security.Claims;
using System.Text;

namespace Swim_Feedback.Shared
{
    public class ChangePasswordFormModel
    {
        [Required(ErrorMessage = "Een wachtwoord is vereist.")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Het wachtwoord moet tussen de 6 en 50 karakters zijn.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@#$%^&+=!]).*$",
            ErrorMessage = "Het wachtwoord moet ten minste een kleine letter, grote letter, nummer en speciaal karakter bevatten.")]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "De wachtwoord bevestiging is vereist.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "De wachtwoord bevestiging komt niet overeen met het wachtwoord.")]
        public string ConfirmPassword { get; set; } = "";
    }

    public partial class CChangePassword : ComponentBase
    {
        [Parameter]
        public IdentityUser User { get; set; }

        [Inject]
        private IDbContextFactory<ApplicationDbContext>? dbContextFactory { get; set; }
        [Inject]
        private UserManager<IdentityUser>? userManager { get; set; }
        [Inject]
        private AuthenticationStateProvider authenticationStateProvider { get; set; }

        [Inject]
        private NavigationManager navigationManager { get; set; }

        private IdentityUser? currentUser;

        private EditContext editContext;
        public ValidationMessageStore messages;

        private ChangePasswordFormModel changePasswordForm = new();

        protected override async Task OnInitializedAsync()
        {
            editContext = new EditContext(changePasswordForm);
            editContext.OnFieldChanged += EditContext_OnFieldChanged;
            messages = new ValidationMessageStore(editContext);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                ClaimsPrincipal claimsPrincipal = (await authenticationStateProvider.GetAuthenticationStateAsync()).User;
                if (claimsPrincipal.Identity != null && claimsPrincipal.Identity.IsAuthenticated)
                {
                    currentUser = await userManager.GetUserAsync(claimsPrincipal);
                }
            }
        }

        private void EditContext_OnFieldChanged(object sender, FieldChangedEventArgs e)
        {
            messages.Clear();

            if (User == null)
            {
                messages.Add(e.FieldIdentifier, "Geen account geselecteerd");
            }
        }

        private async Task ChangePasswordAsync()
        {
            string encodedId = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(User.Id));
            string encodedPassword = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(changePasswordForm.Password));

            ApplicationDbContext dbContext = await dbContextFactory.CreateDbContextAsync();

            await dbContext.AdminLogs.AddAsync(new(
                currentUser.Id,
                User.Id,
                $"{currentUser.UserName} heeft het wachtwoord van {User.UserName} aangepast."));
            await dbContext.SaveChangesAsync();

            navigationManager.NavigateTo("/RResetPassword/" + encodedId + "/" + encodedPassword, true);
        }
    }
}
