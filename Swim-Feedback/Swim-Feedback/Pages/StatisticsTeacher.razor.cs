using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using Swim_Feedback.Data;
using Swim_Feedback.Services;

namespace Swim_Feedback.Pages
{
    public partial class StatisticsTeacher : ComponentBase
    {
        [Inject]
        private IDbContextFactory<ApplicationDbContext>? dbContextFactory { get; set; }

        [Inject]
        private StatisticsService? statisticsService { get; set; }

        [Inject]
        private NavigationManager navigationManager { get; set; }

        private List<string>? topics;

        private List<SwimClass>? swimClasses;

        private string? swimClassId;

        [Inject]
        private IJSRuntime? JS { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ApplicationDbContext dbContext = await dbContextFactory.CreateDbContextAsync();

            swimClasses = dbContext.SwimClasses.ToList();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                ApplicationDbContext dbContext = await dbContextFactory.CreateDbContextAsync();

                List<string> tempTopics = await dbContext.StudentFeedbacks
                    .Select(sf => sf.Topic)
                    .GroupBy(sf => sf)
                    .Select(sf => sf.Key)
                    .ToListAsync();

                topics = tempTopics.Where(t => !t.IsNullOrEmpty()).ToList();

                List<int> scoreChartData = GetScoreChartData(null);

                await JS.InvokeAsync<string>("statistics.init", scoreChartData);

                StateHasChanged();
            }
        }

        private List<int> GetScoreChartData(string? topic)
        {
            if (topic == "Geen") topic = null;

            //List<PeriodData> yearlyStats = statisticsService.GetPeriodStatistics(topic, null, null);

            //List<int> scoreChartData = new();
            //foreach (PeriodData period in yearlyStats)
            //{
            //    scoreChartData.Add(period.Amount);
            //}

            Dictionary<string, int> yearlyStats = new Dictionary<string, int>
                    {
                        { "0-10", 24 },
                        { "11-20", 26 },
                        { "21-30", 21 },
                        { "31-40", 30 },
                        { "41-50", 35 },
                        { "51-60", 50 },
                        { "61-70", 70 },
                        { "71-80", 82 },
                        { "81-90", 81 },
                        { "91-100", 75 }
                    };

            List<int> scoreChartData = new()
            {
                yearlyStats["0-10"],
                yearlyStats["11-20"],
                yearlyStats["21-30"],
                yearlyStats["31-40"],
                yearlyStats["41-50"],
                yearlyStats["51-60"],
                yearlyStats["61-70"],
                yearlyStats["71-80"],
                yearlyStats["81-90"],
                yearlyStats["91-100"]
            };

            return scoreChartData;
        }

        private async Task TopicChanged(ChangeEventArgs e)
        {
            List<int> scoreChartData = GetScoreChartData(e.Value.ToString());

            await JS.InvokeAsync<string>("teacherStatistics.updateScoreChart", scoreChartData);
        }
    }
}
