using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Swim_Feedback.Pages
{
    public class RSignInModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public RSignInModel(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task OnGetAsync(string userName, string password)
        {
            userName = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(userName));
            password = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(password));

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            await _signInManager.PasswordSignInAsync(userName, password, false, false);

            Response.Redirect("/dashboard");
        }
    }
}
