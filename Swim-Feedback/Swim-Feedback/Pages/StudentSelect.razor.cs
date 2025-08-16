using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using Swim_Feedback.Data;
using System.Text;

namespace Swim_Feedback.Pages
{
    public partial class StudentSelect : ComponentBase
    {
        [Parameter]
        public long SwimClassId { get; set; }
        [Parameter]
        public string? EncodedTopic { get; set; }

        [Inject]
        public IDbContextFactory<ApplicationDbContext>? DbContextFactory { get; set; }
        [Inject]
        private IJSRuntime? js { get; set; }
        [Inject]
        private NavigationManager? navigationManager { get; set; }
        [Inject]
        private UserManager<IdentityUser>? userManager { get; set; }

        private List<Student>? students;

        private EditContext editContext;
        public ValidationMessageStore messages;

        private LoginFormModel loginForm = new();

        protected override void OnInitialized()
        {
            editContext = new EditContext(loginForm);
            editContext.OnFieldChanged += EditContext_OnFieldChanged;
            messages = new ValidationMessageStore(editContext);
        }

        protected override async Task OnInitializedAsync()
        {
            ApplicationDbContext dbContext = await DbContextFactory.CreateDbContextAsync();

            students = await dbContext.Students
                .Where(s => s.SwimClassId == SwimClassId)
                .Where(s => s.Name != "")
                .Include(s => s.StudentImage)
                .ToListAsync();
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

                if (!Task.Run(async () => await userManager.CheckPasswordAsync(user, loginForm.Password)).GetAwaiter().GetResult())
                {
                    messages.Add(e.FieldIdentifier, "De E-mailadres en wachtwoord combinatie is incorrect.");
                }
            }
        }

        private void BackToDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }

        private void SelectStudent(long studentId)
        {
            navigationManager.NavigateTo("/feedback/" + SwimClassId + "/" + studentId + "/" + EncodedTopic);
        }

        private async Task OpenPopup()
        {
            await js.InvokeAsync<string>("studentSelect.openPopup");
        }

        private async Task ClosePopup()
        {
            await js.InvokeAsync<string>("studentSelect.closePopup");
        }
    }
}