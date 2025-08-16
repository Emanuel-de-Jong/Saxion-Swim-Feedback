using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.JSInterop;
using Swim_Feedback.Data;
using Swim_Feedback.Models;
using System.Net;
using System.IO;
using System.ComponentModel;
using System.Data;
using OfficeOpenXml;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.JSInterop;
using LicenseContext = OfficeOpenXml.LicenseContext;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.IdentityModel.Tokens;
using CsvHelper;
using System.Globalization;

namespace Swim_Feedback.Pages
{
    public partial class AdminDashboard : ComponentBase
    {
        [Parameter]
        public bool IsReload { get; set; } = false;

        [Inject]
        private NavigationManager navigationManager { get; set; }
        [Inject]
        public IDbContextFactory<ApplicationDbContext>? dbContextFactory { get; set; }

        [Inject]
        private UserManager<IdentityUser>? userManager { get; set; }
        [Inject]
        private AuthenticationStateProvider? authenticationStateProvider { get; set; }
        [Inject]
        private IJSRuntime? js { get; set; }

        private List<IdentityUser>? users;
        private List<IdentityRole>? roles;
        private List<IdentityUserRole<string>>? userRoles;
        private List<AdminLog>? adminLogs;

        private IdentityUser? currentUser;
        private IdentityUser? selectedUser;

        protected override async Task OnInitializedAsync()
        {
            ClaimsPrincipal claimsPrincipal = (await authenticationStateProvider.GetAuthenticationStateAsync()).User;
            if (claimsPrincipal.Identity != null && claimsPrincipal.Identity.IsAuthenticated)
            {
                currentUser = await userManager.GetUserAsync(claimsPrincipal);
            }

            ApplicationDbContext dbContext = await dbContextFactory.CreateDbContextAsync();

            roles = await dbContext.Roles.ToListAsync();
            users = await dbContext.Users.ToListAsync();
            userRoles = await dbContext.UserRoles.ToListAsync();
            adminLogs = await dbContext.AdminLogs.ToListAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //await js.InvokeAsync<string>("adminDashboard.init");

            if (firstRender)
            {
                if (!IsReload)
                {
                    //navigationManager.NavigateTo(navigationManager.Uri + "/true", forceLoad: true);
                }
            }
        }

        public async Task GiveUserRoleAsync(IdentityUser user, IdentityRole role)
        {
            user = await userManager.FindByIdAsync(user.Id);

            AdminLog adminLog = new(currentUser.Id, user.Id);
            if (userRoles.Any(Ur => Ur.UserId == user.Id && Ur.RoleId == role.Id))
            {
                await userManager.RemoveFromRoleAsync(user, role.Name);
                adminLog.Message = $"{currentUser.UserName} heeft de rol {role.Name} van {user.UserName} afgenomen.";
            } else
            {
                await userManager.AddToRoleAsync(user, role.Name);
                adminLog.Message = $"{currentUser.UserName} heeft de rol {role.Name} aan {user.UserName} gegeven.";
            }

            ApplicationDbContext dbContext = await dbContextFactory.CreateDbContextAsync();

            await dbContext.AdminLogs.AddAsync(adminLog);
            await dbContext.SaveChangesAsync();

            userRoles = await dbContext.UserRoles.ToListAsync();
            adminLogs = await dbContext.AdminLogs.ToListAsync();

            StateHasChanged();
        }

        private void SetSelectedUser(IdentityUser user)
        {
            selectedUser = user;
        }

        //The DeleteData function removes the studentfeedback, teacherfeedback and student data from the Database.
        public ResponseMessage<string> DeleteData(Student student)
    {
            ApplicationDbContext dbContext = dbContextFactory.CreateDbContext();

            if (!dbContext.Students.Contains(student))
            {
                return new ResponseMessage<string>(HttpStatusCode.BadRequest, "Student does not exist","");
            }

            var feedbacksToRemove = dbContext.StudentFeedbacks
                .Where(feedback => feedback.Student == student);
            dbContext.StudentFeedbacks.RemoveRange(feedbacksToRemove);

            var teacherFeedbacksToRemove = dbContext.TeacherFeedbacks
                .Where(feedback => feedback.Student == student);
            dbContext.TeacherFeedbacks.RemoveRange(teacherFeedbacksToRemove);

            var studentToRemove = dbContext.Students
                .Where(students => students == student);
            dbContext.Students.RemoveRange(studentToRemove);

            dbContext.SaveChanges();

            return new ResponseMessage<string>(HttpStatusCode.OK, "Data has been removed", "");
        }

        //This function imports data from a csv file and saves it as a List<String[]>
        public async Task ImportFromCSV(InputFileChangeEventArgs e)
        {
            if (e != null && Path.GetExtension(e.File.Name).Equals(".csv", StringComparison.OrdinalIgnoreCase))
            {
                var stream = e.File.OpenReadStream();
                List<String[]> csv = new List<string[]>();
                MemoryStream ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                stream.Close();
                var outputFileString = System.Text.Encoding.UTF8.GetString(ms.ToArray());

                var lines = outputFileString.Split(Environment.NewLine);

                for (int i = 1; i < lines.Length; i++)
                {
                    csv.Add(lines[i].Split(';'));
                }
                SaveCSVData(csv);
            }
        }

        //This function saves the data from the import csv function into the database.
        private void SaveCSVData(List<String[]> csvData)
        {
            var dbContext = dbContextFactory.CreateDbContext();

            foreach (var line in csvData)
            {
                int klantnumer = int.Parse(line[0]);
                string voornaam = line[3];
                string[] klasnaamData = line[4].ToString().Split(',');
                string klasnaam = klasnaamData[0];

                if(klantnumer == 0 || voornaam.IsNullOrEmpty() || voornaam.IsNullOrEmpty() || klasnaam.IsNullOrEmpty()) { continue; }

                if (!klasnaam.IsNullOrEmpty())
                {
                    SwimClass swimClass;
                    if (!dbContext.Set<SwimClass>().Any(e => e.Name == klasnaam))
                    {
                        swimClass = new SwimClass(klasnaam);
                        dbContext.SwimClasses.Add(swimClass);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        swimClass = dbContext.SwimClasses.Find(klasnaam);
                    }

                    if(!dbContext.Set<Student>().Any(s => s.CustomerNumber == klantnumer || s.Name == voornaam))
                    {
                        Student student = new Student(klantnumer, voornaam);
                        student.SwimClass = swimClass;
                        dbContext.Students.Add(student);
                    }

                    dbContext.SaveChanges();
                }
            }
        }

        private async Task OpenPasswordPopup()
        {
            await js.InvokeAsync<string>("adminDashboard.openPasswordPopup");
        }

        private async Task ClosePasswordPopup()
        {
            await js.InvokeAsync<string>("adminDashboard.closePasswordPopup");
        }

        private async Task OpenImportPopup()
        {
            await js.InvokeAsync<string>("adminDashboard.openImportPopup");
        }

        private async Task CloseImportPopup()
        {
            await js.InvokeAsync<string>("adminDashboard.closeImportPopup");
        }
    }
}
