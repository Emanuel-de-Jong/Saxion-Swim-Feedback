using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.RegularExpressions;

namespace Swim_Feedback.Pages
{
    public class RRegisterModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;

        public RRegisterModel(UserManager<IdentityUser> userManager, IUserStore<IdentityUser> userStore)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = (IUserEmailStore<IdentityUser>)_userStore;
        }

        public async Task OnGetAsync(string encodedEmail, string encodedPassword, bool shouldLogin, string? encodedRedirect)
        {
            string email = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(encodedEmail));
            string password = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(encodedPassword));
            string redirect = "/";
            if (encodedRedirect != null)
            {
                redirect = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(encodedRedirect));
            }

            if (!IsEmailValid(email) || !IsPasswordValid(password)) return;

            IdentityUser user = new();

            await _userStore.SetUserNameAsync(user, email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, email, CancellationToken.None);
            IdentityResult result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                await _userManager.ConfirmEmailAsync(user, code);

                if (shouldLogin)
                {
                    Response.Redirect("/RSignIn/" + encodedEmail + "/" + encodedPassword);
                }
                else
                {
                    Response.Redirect(redirect);
                }
            }
        }

        private bool IsEmailValid(string email)
        {
            if (email.Length < 6 || email.Length > 25) return false;

            Regex regex = new("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{1,4}$");
            if (!regex.IsMatch(email)) return false;

            return true;
        }

        private bool IsPasswordValid(string password)
        {
            if (password.Length < 6 || password.Length > 50) return false;

            Regex regex = new(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@#$%^&+=!]).*$");
            if (!regex.IsMatch(password)) return false;

            return true;
        }
    }
}
