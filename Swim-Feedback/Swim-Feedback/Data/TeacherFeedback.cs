using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Swim_Feedback.Data
{
    public class TeacherFeedback
    {
        [Key]
        public long TeacherFeedbackId { get; set; }
        public string? AccountId { get; set; }
        public IdentityUser? Account { get; set; }
        public long? SwimClassId { get; set; }
        public SwimClass? SwimClass { get; set; }
        public long? StudentId { get; set; }
        public Student? Student { get; set; }
        public string Topic { get; set; }
        public string Notes { get; set; }
        public DateTimeOffset Date { get; set; }

        public TeacherFeedback(string topic, string notes, DateTimeOffset date)
        {
            Topic = topic;
            Notes = notes;
            Date = date;
        }
    }
}
