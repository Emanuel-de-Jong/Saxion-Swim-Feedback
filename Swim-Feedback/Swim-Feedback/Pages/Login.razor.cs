using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Swim_Feedback.Pages
{
    public class LoginFormModel
    {
        [Required(ErrorMessage = "Een E-mailadres is vereist.")]
        [StringLength(25, MinimumLength = 6, ErrorMessage = "Het E-mailadres moet tussen de 6 en 26 karakters zijn.")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Een wachtwoord is vereist.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";
    }

    public partial class Login : ComponentBase
    {
        [Inject]
        private IJSRuntime? js { get; set; }

        [Inject]
        private NavigationManager? navigationManager { get; set; }
        [Inject]
        private UserManager<IdentityUser>? userManager { get; set; }

        private EditContext editContext;
        public ValidationMessageStore messages;

        private LoginFormModel loginForm = new();

        protected override void OnInitialized()
        {
            editContext = new EditContext(loginForm);
            editContext.OnFieldChanged += EditContext_OnFieldChanged;
            messages = new ValidationMessageStore(editContext);
        }

        private void EditContext_OnFieldChanged(object sender, FieldChangedEventArgs e)
        {
            if (e.FieldIdentifier.FieldName == "Email" || e.FieldIdentifier.FieldName == "Password")
            {
                messages.Clear();

                IdentityUser? user = Task.Run(async () => await userManager.FindByEmailAsync(loginForm.Email)).GetAwaiter().GetResult();
                if (user == null)
                {
                    messages.Add(e.FieldIdentifier, "Geef een geldig E-mailadres");
                    return;
                }

                if (!Task.Run(async () => await userManager.CheckPasswordAsync(user, loginForm.Password)).GetAwaiter().GetResult()) {
                    messages.Add(e.FieldIdentifier, "De E-mailadres en wachtwoord combinatie is incorrect.");
                }
            }
        }

        private void LoginUser()
        {
            string encodedUserName = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(loginForm.Email));
            string encodedPassword = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(loginForm.Password));

            navigationManager.NavigateTo("RSignIn/" + encodedUserName + "/" + encodedPassword, true);
        }

        private async Task OpenPopup()
        {
            await js.InvokeAsync<string>("login.openPopup");
        }

        private async Task ClosePopup()
        {
            await js.InvokeAsync<string>("login.closePopup");
        }
    }
}
