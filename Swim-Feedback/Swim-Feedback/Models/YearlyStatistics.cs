using System.Net;

namespace Swim_Feedback.Models
{
    public class YearlyStatistics
    {
        public int Year { get; set; }

        public Dictionary<string, int> Statistics { get; set; }
        public YearlyStatistics(int year, Dictionary<string, int> stats)
        {
            Year = year;
            Statistics = stats;
        }
    }
}
