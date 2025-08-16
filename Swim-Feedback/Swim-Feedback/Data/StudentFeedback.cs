using NuGet.Packaging.Signing;
using System.ComponentModel.DataAnnotations;

namespace Swim_Feedback.Data
{
    public class StudentFeedback
    {
        [Key]
        public long StudentFeedbackId { get; set; }
        public long? StudentId { get; set; }
        public Student? Student { get; set; }
        public string? Topic { get; set; }
        public int Score { get; set; }
        public DateTimeOffset Date { get; set; }

        public StudentFeedback(string? topic, int score, DateTimeOffset date)
        {
            Topic = topic;
            Score = score;
            Date = date;
        }

        public StudentFeedback(Student student, string? topic, int score, DateTimeOffset date)
        {
            Student = student;
            Topic = topic;
            Score = score;
            Date = date;
        }
    }
}
