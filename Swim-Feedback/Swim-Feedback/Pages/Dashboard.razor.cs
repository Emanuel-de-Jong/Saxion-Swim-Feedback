using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Swim_Feedback.Data;
using Swim_Feedback.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Swim_Feedback.Models;

namespace Swim_Feedback.Pages
{
    public partial class Dashboard : ComponentBase
    {
        [Inject]
        private IDbContextFactory<ApplicationDbContext>? dbContextFactory { get; set; }
        [Inject]
        private IJSRuntime? JS { get; set; }
        [Inject]
        private StatisticsService? statisticsService { get; set; }
        [Inject]
        private NavigationManager navigationManager { get; set; }

        private List<string>? topics;

        private string? feedbackTopic;

        private List<SwimClass>? swimClasses;

        private string? feedbackSwimClassId;

        private string? topic;

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

                await JS.InvokeAsync<string>("dashboard.init", scoreChartData);

                StateHasChanged();
            }
        }

        private void CreateFeedback()
        {
            string? encodedTopic = null;
            if (!feedbackTopic.IsNullOrEmpty())
            {
                encodedTopic = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(feedbackTopic));
            }

            navigationManager.NavigateTo("student-select/" + feedbackSwimClassId + "/" + encodedTopic);
        }

        private List<int> GetScoreChartData()
        {
            string? tempTopic = topic?.Replace("-", " ");
            if (tempTopic == "") tempTopic = null;

            Dictionary<int, int> stats = statisticsService.GetPeriodStatistics(tempTopic, null, null);

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

            await JS.InvokeAsync<string>("dashboard.updateScoreChart", scoreChartData);
        }

        private async Task ToggleTooltipFeedback()
        {
            await JS.InvokeAsync<string>("dashboard.toggleTooltipFeedback");
        }

        private async Task ToggleTooltipStatistics()
        {
            await JS.InvokeAsync<string>("dashboard.toggleTooltipStatistics");
        }
    }
}
