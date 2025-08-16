using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swim_Feedback.Data;
using Swim_Feedback.Services;
using System.Text;

namespace Swim_Feedback.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject]
        private TestDataService testDataService { get; set; }

        [Inject]
        private RoleManager<IdentityRole> roleManager { get; set; }
        [Inject]
        private UserManager<IdentityUser> userManager { get; set; }

        [Inject]
        private NavigationManager navigationManager { get; set; }

        [Inject]
        private StatisticsService statisticsService { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await AddBaseDataAsync();
                //testDataService.AddTestData();
                Console.WriteLine("------------------------------");
                statisticsService.GetPeriodStatistics(null,null,null);
                navigationManager.NavigateTo("/login");
            }
        }

        private async Task AddBaseDataAsync()
        {
            // Admin and Teacher roles
            string adminRoleName = "Admin";
            string teacherRoleName = "Teacher";

            if (!await roleManager.RoleExistsAsync(adminRoleName))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRoleName));
            }

            if (!await roleManager.RoleExistsAsync(teacherRoleName))
            {
                await roleManager.CreateAsync(new IdentityRole(teacherRoleName));
            }

            // User
            string adminUserEmail = "admin@admin.com";
            string adminUserPassword = "@Password1";

            IdentityUser? user = await userManager.FindByNameAsync(adminUserEmail);
            if (user == null)
            {
                string encodedEmail = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(adminUserEmail));
                string encodedPassword = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(adminUserPassword));

                navigationManager.NavigateTo("RRegister/" + encodedEmail + "/" + encodedPassword + "/" + false, true);

                user = await userManager.FindByNameAsync(adminUserEmail);
            }

            // Make user admin
            if (!await userManager.IsInRoleAsync(user, adminRoleName))
            {
                await userManager.AddToRoleAsync(user, adminRoleName);
            }
        }
    }
}
