using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using Swim_Feedback.Data;
using Swim_Feedback.Services;

namespace Swim_Feedback.Pages
{
    public partial class Statistics : ComponentBase
    {
        [Inject]
        private IDbContextFactory<ApplicationDbContext>? dbContextFactory { get; set; }

        [Inject]
        private StatisticsService? statisticsService { get; set; }


        private List<string>? topics;

        private List<SwimClass>? swimClasses;

        private string? topic;

        private string? swimClass;

        private DateTimeOffset? startDate;
        private DateTimeOffset? endDate;

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
                List<int> scoreChartData = GetScoreChartData();

                await JS.InvokeAsync<string>("statistics.init", scoreChartData);

                StateHasChanged();
            }
        }

        private List<int> GetScoreChartData()
        {
            string? tempTopic = topic?.Replace("-", " ");
            if (tempTopic == "") tempTopic = null;

            string? tempSwimClass = swimClass?.Replace("-", " ");
            if (tempSwimClass == "") tempSwimClass = null;

            Dictionary<int, int> stats = statisticsService.GetPeriodStatistics(tempTopic, startDate, endDate);

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
            List<int> scoreChartData = GetScoreChartData();

            await JS.InvokeAsync<string>("statistics.updateScoreChart", scoreChartData);
        }
    }
}
