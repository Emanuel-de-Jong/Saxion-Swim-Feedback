using Microsoft.EntityFrameworkCore;
using Swim_Feedback.Data;
using Swim_Feedback.Services;


namespace Swim_Feedback_Tests.Services
{
    [TestClass]
    public class TestStatisticService
    {
        public TestStatisticService() {
            DbContextFactory = new MyDbContextFactory();
        }

        private IDbContextFactory<ApplicationDbContext>? DbContextFactory { get; set; }


        private StatisticsService statisticsService { get; set; }

        //Deze functie's crashen omdat het ons niet is gelukt om de IServiceScopeFactory te assignen
        [TestMethod]
        public void TestGetPeriodStatistics()
        {
            var dbContext = DbContextFactory.CreateDbContext();

            int totalfeedbackcount = dbContext.StudentFeedbacks.Count();

            Dictionary<int, int> statistics = statisticsService.GetPeriodStatistics(null, null, null);

            int sum = 0;

            foreach (int value in statistics.Values)
            {
                sum += value;
            }

            Assert.IsTrue(totalfeedbackcount == sum);
        }


    }
}
