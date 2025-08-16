using System.Net;

namespace Swim_Feedback.Models
{
    public class CsvData
    {
        public int CustomerID { get; set; }
        public int Firstname { get; set; }
        public int Infix { get; set; }
        public int Lastname { get; set; }
        public string groep_lesson_StarDate {get; set; }
        public DateTimeOffset StarDate { get; set; }

    }
}
