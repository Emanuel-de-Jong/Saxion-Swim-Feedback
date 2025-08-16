using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using Swim_Feedback.Data;
using Swim_Feedback.Services;

namespace Swim_Feedback.Pages
{
    public partial class StatisticsStudent : ComponentBase
    {
        [Inject]
        private IDbContextFactory<ApplicationDbContext>? dbContextFactory { get; set; }

        [Inject]
        private StatisticsService? statisticsService { get; set; }

        [Inject]
        private NavigationManager navigationManager { get; set; }

        private List<string>? topics;

        private List<SwimClass>? swimClasses;

        private string? topic;

        private List<Student>? students;

        private string? studentId;

        [Inject]
        private IJSRuntime? JS { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ApplicationDbContext dbContext = await dbContextFactory.CreateDbContextAsync();

            List<string> tempTopics = await dbContext.StudentFeedbacks
                .Select(sf => sf.Topic)
                .GroupBy(sf => sf)
                .Select(sf => sf.Key)
                .ToListAsync();

            topics = tempTopics.Where(t => !t.IsNullOrEmpty()).ToList();

            swimClasses = dbContext.SwimClasses.ToList();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                List<int> scoreChartData = await GetScoreChartData();

                await JS.InvokeAsync<string>("studentStatistics.init", scoreChartData);

                StateHasChanged();
            }
        }

        private async Task<List<int>> GetScoreChartData()
        {
            string? tempTopic = topic?.Replace("-", " ");
            if (tempTopic == "") tempTopic = null;

            Dictionary<int, int>? stats;

            if (!studentId.IsNullOrEmpty())
            {
                ApplicationDbContext dbContext = await dbContextFactory.CreateDbContextAsync();
                Student? student = await dbContext.Students.FindAsync(long.Parse(studentId));

                stats = statisticsService.GetStudentStatistics(student, tempTopic);
            } else
            {
                stats = new()
                {
                    { 1, 0 },
                    { 2, 0 },
                    { 3, 0 },
                    { 4, 0 },
                    { 5, 0 },
                    { 6, 0 },
                    { 7, 0 },
                    { 8, 0 },
                    { 9, 0 },
                    { 10, 0 }
                };
            }

            List<int> scoreChartData = new()
            {
                stats[1],
                stats[2],
                stats[3],
                stats[4],
                stats[5],
                stats[6],
                stats[7],
                stats[8],
                stats[9],
                stats[10]
            };

            return scoreChartData;
        }

        private async Task FilterChanged()
        {
            List<int> scoreChartData = await GetScoreChartData();

            await JS.InvokeAsync<string>("studentStatistics.updateScoreChart", scoreChartData);
        }

        private async Task SwimClassChanged(ChangeEventArgs e)
        {
            long swimClassId = long.Parse(e.Value.ToString());

            ApplicationDbContext dbContext = await dbContextFactory.CreateDbContextAsync();

            List<Student> tempStudents = await dbContext.Students
                .Where(s => s.SwimClassId == swimClassId)
                .ToListAsync();

            students = tempStudents.Where(s => s.CustomerNumber != 0).ToList();
        }

    }
}
