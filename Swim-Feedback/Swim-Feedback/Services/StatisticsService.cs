using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Swim_Feedback.Data;
using Swim_Feedback.Models;
using System.Net;

namespace Swim_Feedback.Services
{
    public class StatisticsService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public StatisticsService(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        /*        //The getYearlyStats function returns the statistics of a specific year. It returns a YearlyStatistic this is a int year and a Dictionary<string, int> that contains the data. The function also contains 2 parameters that can be used to filter the data
                public ResponseMessage<YearlyStatistics> GetYearlyStats(int? year, string? topic)
                {
                    int selectedyear = year == null ? DateTime.Now.Year : year.Value;

                    using IServiceScope scope = serviceScopeFactory.CreateScope();
                    ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var feedbacks = dbContext.StudentFeedbacks
                        .Where(feedback => feedback.Date.Year == selectedyear)
                        .Where(f => topic.IsNullOrEmpty() ? true : f.Topic == topic)
                        .ToList();

                    var count = new Dictionary<string, int>
                    {
                        { "0-10", 0 },
                        { "11-20", 0 },
                        { "21-30", 0 },
                        { "31-40", 0 },
                        { "41-50", 0 },
                        { "51-60", 0 },
                        { "61-70", 0 },
                        { "71-80", 0 },
                        { "81-90", 0 },
                        { "91-100", 0 }
                    };

                    foreach (var feedback in feedbacks)
                    {
                        foreach (var range in count.Keys)
                        {
                            var scoreRange = range.Split('-').Select(int.Parse).ToArray();
                            if (feedback.Score >= scoreRange[0] && feedback.Score <= scoreRange[1])
                            {
                                count[range]++;
                                break;
                            }
                        }
                    }

                    var yearlyStatistics = new YearlyStatistics(selectedyear, count);
                    return new ResponseMessage<YearlyStatistics>(HttpStatusCode.OK, "", yearlyStatistics);
                }*/

        //The AverageScorePerPeriod function returns the average score over a certain period. This period can be set in the parameters starDate and endDate. If no period is given it will return the average score of all time.
        //The function is also contains a paramater for class so that it can be used to filter the data.
        //The function returns a Dictionary containing a int and a double. The int is the year and the double is the average score/
        public Dictionary<int, double> AverageScorePerPeriod(string? Class, DateTimeOffset? startDate, DateTimeOffset? endDate)
        {
            List<StudentFeedback> studenfeedback = null;
            if (Class == null)
            {
                studenfeedback = getStudentFeedbackBasedOnFilters(null, null, null, null, null, null, null);
            }
            else
            {
                if (startDate == null & endDate == null)
                {
                    studenfeedback = getStudentFeedbackBasedOnFilters(null, Class, null, null, null, null, null);
                }
                else
                {
                    studenfeedback = getStudentFeedbackBasedOnFilters(null, Class, startDate, endDate, null, null, null);
                }
            }

            Dictionary<int, double> averagePerYear = studenfeedback
                    .GroupBy(f => f.Score) // Group by score
                    .ToDictionary(
                                    g => g.Key,
                                    g => g.Average(f => f.Score)
                      );

            return averagePerYear;
        }

        //The getPeriodStatisticsfunction returns the count of the scores over a certain period. This period can be set in the parameters starDate and endDate. If no period is given it will return the average score of all time.
        //The function is also contains a paramater for Topic so that it can be used to filter the data.
        //The function returns a list of YearlyData. A Yearly data contains a int score and int amount.
        public Dictionary<int,int> GetPeriodStatistics(string? Topic, DateTimeOffset? startDate, DateTimeOffset? endDate)
        {
            List<StudentFeedback> studentfeedback = null;
            if (Topic == null)
            {
                studentfeedback = getStudentFeedbackBasedOnFilters(null, null, null, null, null, null, null);
            }
            else
            {
                if (startDate == null & endDate == null)
                {
                    studentfeedback = getStudentFeedbackBasedOnFilters(Topic, null, null, null, null, null, null);
                }
                else
                {
                    studentfeedback = getStudentFeedbackBasedOnFilters(Topic, null, startDate, endDate, null, null, null);
                }
            }

            Dictionary<int, int> scoreCounts = CreateScoreCount(studentfeedback);

            return scoreCounts;
        }

        //This function creates a Dictionary of statistics from a list of studentfeedbacks
        private Dictionary<int,int> CreateScoreCount(List<StudentFeedback> studentfeedback)
        {
            Dictionary<int, int> scoreCounts = new Dictionary<int, int>
            {
                {1,0},
                {2,0},
                {3,0},
                {4,0},
                {5,0},
                {6,0},
                {7,0},
                {8,0},
                {9,0},
                {10,0}
            };

            var tempScoreCount = studentfeedback
                    .GroupBy(f => f.Score) // Group by score
                    .ToDictionary(g => g.Key, g => g.Count());

            foreach (var score in tempScoreCount)
            {
                scoreCounts[score.Key] = score.Value;
            }

            return scoreCounts;
        }

        //The GetPositiveFeedbackPercentagesByYear function returns to percentage amount of positive feedback per year. It counts the positive feedback by looking if the score is higher than 6.
        public Dictionary<int,double> GetPositiveFeedbackPercentagesByYear()
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var feedbackDataByYear = dbContext.StudentFeedbacks
                .GroupBy(f => f.Date.Year)
                .ToDictionary(
                    g => g.Key,
                    g => g.Count(f => f.Score > 55) > 0
                        ? (double)g.Count(f => f.Score > 55) / g.Count() * 100
                        : 0
                );


            return feedbackDataByYear;
        }

        //The getStudentStats function retrieves a dictionary that counts every score that a specific student has given. The data can also be filtered by a subject
        public Dictionary<int, int> GetStudentStatistics(Student student, string? subject)
        {
            if (student == null)
            {
                return null;
            }

            List<StudentFeedback> studentfeedback = null;
            if (subject == null)
            {
                studentfeedback = getStudentFeedbackBasedOnFilters(null, null, null, null, null, student, null);
            }
            else
            {
                studentfeedback = getStudentFeedbackBasedOnFilters(subject, null, null, null, null, student, null);
            }

            Dictionary<int, int> scoreCounts = CreateScoreCount(studentfeedback);

            return scoreCounts;
        }

/*        //The getStudent function returns a student based on the name of a student
        private Student getStudent(string studentName)
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            return dbContext.Students
             .Where(s => s.Name == studentName)
             .FirstOrDefault();
        }*/

/*        //The getStudentFeedback returns a list of studentFeedback based on the students name and a subject
        private List<StudentFeedback> getStudentFeedback(Student student, string subject)
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (subject.IsNullOrEmpty())
            {

                return dbContext.StudentFeedbacks
                 .Include(feedback => feedback.Student)
                 .Where(feedback => feedback.StudentId == student.StudentId)
                 .ToList();
            }

            return dbContext.StudentFeedbacks
            .Include(feedback => feedback.Student)
            .Where(feedback => feedback.StudentId == student.StudentId && feedback.Topic == subject)
            .ToList();

        }
*/


/*        public ResponseMessage<List<Feedback>> getGlobalFeedback(string? Topic, string? Class, DateTimeOffset? StartDate, DateTimeOffset? TillDate)
        {
            List<StudentFeedback> feedback = getStudentFeedbackBasedOnFilters(Topic, Class, StartDate, TillDate, null, null, null);
            // feedback is onnodig
            List<Feedback> feedbackList = feedback.Select(sf => new Feedback
            {
                StudentName = sf.Student?.Name,
                Topic = sf.Topic,
                Score = sf.Score,
                Date = sf.Date
            }).ToList();

            return new ResponseMessage<List<Feedback>>(HttpStatusCode.OK, "", feedbackList);
        }*/

        //The getStudentFeedbackBasedOnFilters function is a function that returns a list of studentfeedbacks. This function is used to retrieve the data based on a number of filters.
        public List<StudentFeedback> getStudentFeedbackBasedOnFilters(string? Topic, string? Class, DateTimeOffset? StartDate, DateTimeOffset? TillDate, DateTimeOffset? selectedDate, Student? selectedChild, int? Score)
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            return dbContext.StudentFeedbacks
            .Where(sf => !Topic.IsNullOrEmpty() ? (sf.Topic == Topic) : true)
            .Where(sf => !Class.IsNullOrEmpty() ? (sf.Student.SwimClass.Name == Class) : true)
            .Where(sf => StartDate != null ? (sf.Date >= StartDate) : true)
            .Where(sf => TillDate != null ? (sf.Date <= TillDate) : true)
            .Where(sf => selectedDate != null ? (sf.Date == selectedDate) : true)
            .Where(sf => selectedChild!= null ? (sf.Student == selectedChild || sf.StudentId == selectedChild.StudentId) : true)
            .Where(sf => Score != null ? (sf.Score == Score) : true)
            .ToList();

        }
    }
}
