using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Swim_Feedback.Data;
using System.Text;

namespace Swim_Feedback.Pages
{
    public partial class Feedback : ComponentBase
    {
        [Parameter]
        public long SwimClassId { get; set; }
        [Parameter]
        public long StudentId { get; set; }
        [Parameter]
        public string? EncodedTopic { get; set; }
        public string? Topic { get; set; }

        [Inject]
        public IDbContextFactory<ApplicationDbContext>? DbContextFactory { get; set; }
        [Inject]
        private NavigationManager navigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (!EncodedTopic.IsNullOrEmpty())
            {
                Topic = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(EncodedTopic));
            }
        }

        public void SetScore(int score)
        {
            // Make the 1-5 into 1-10
            score *= 2;

            ApplicationDbContext dbContext = DbContextFactory.CreateDbContext();

            //Student student = dbContext.Students.Find(StudentId);

            dbContext.StudentFeedbacks.Add(new StudentFeedback(Topic, score, DateTimeOffset.Now) { StudentId = StudentId });

            dbContext.SaveChanges();

            if(navigationManager != null)
            {
                navigationManager.NavigateTo("/thanks/" + SwimClassId + "/" + EncodedTopic);
            }
        }
    }
}
