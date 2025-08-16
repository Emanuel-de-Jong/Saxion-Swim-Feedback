using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using Swim_Feedback.Data;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Swim_Feedback.Pages
{
    public class RegisterFormModel
    {
        [Required(ErrorMessage = "Een E-mailadres is vereist.")]
        [StringLength(25, MinimumLength = 6, ErrorMessage = "Het E-mailadres moet tussen de 6 en 26 karakters zijn.")]
        [EmailAddress(ErrorMessage = "Geef een geldig E-mailadres")]
        public string Email { get; set; } = "";

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

        [Required]
        [Range(typeof(bool), "true", "true", ErrorMessage = "De Algemene voorwarden moeten geaccepteerd worden.")]
        public bool IsTermsAndConditionsAccepted { get; set; }
    }

    public partial class Register : ComponentBase
    {

        [Inject]
        private IJSRuntime? js { get; set; }

        [Inject]
        private NavigationManager? navigationManager { get; set; }

        [Inject]
        private IDbContextFactory<ApplicationDbContext>? dbContextFactory { get; set; }

        private EditContext editContext;
        public ValidationMessageStore messages;

        private RegisterFormModel registerForm = new();

        protected override void OnInitialized()
        {
            editContext = new EditContext(registerForm);
            editContext.OnFieldChanged += EditContext_OnFieldChanged;
            messages = new ValidationMessageStore(editContext);
        }

        private void EditContext_OnFieldChanged(object sender, FieldChangedEventArgs e)
        {
            if (e.FieldIdentifier.FieldName == "Email")
            {
                messages.Clear();

                ApplicationDbContext dbContext = dbContextFactory.CreateDbContext();

                if (dbContext.Users
                    .Where(u => u.NormalizedEmail == registerForm.Email.ToUpper())
                    .Any())
                {
                    messages.Add(e.FieldIdentifier, "Het E-mailadres wordt al gebruikt.");
                }
            }
        }

        public async Task CreateUserAsync()
        {
            string encodedEmail = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(registerForm.Email));
            string encodedPassword = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(registerForm.Password));

            navigationManager.NavigateTo("RRegister/" + encodedEmail + "/" + encodedPassword + "/" + true, true);
        }

        private async Task OpenPopup()
        {
            await js.InvokeAsync<string>("register.openPopup");
        }

        private async Task ClosePopup()
        {
            await js.InvokeAsync<string>("register.closePopup");
        }
    }
}
