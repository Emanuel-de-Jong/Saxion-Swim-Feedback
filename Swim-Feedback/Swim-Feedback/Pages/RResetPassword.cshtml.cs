using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Swim_Feedback.Shared;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace Swim_Feedback.Pages
{
    [Authorize(Roles = "Admin")]
    public class RResetPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public RResetPasswordModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task OnGetAsync(string encodedId, string encodedPassword)
        {
            string id = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(encodedId));
            string password = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(encodedPassword));

            if (!IsPasswordValid(password)) return;

            IdentityUser user = await _userManager.FindByIdAsync(id);
            if (user == null) return;

            string code = await _userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult result = await _userManager.ResetPasswordAsync(user, code, password);
            if (result.Succeeded)
            {
                Console.WriteLine("Reset password success");
            } else
            {
                Console.WriteLine("Reset password fail");
            }

            Response.Redirect("/admin-dashboard");
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
