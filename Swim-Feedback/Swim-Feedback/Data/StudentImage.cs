using System.ComponentModel.DataAnnotations;

namespace Swim_Feedback.Data
{
    public class StudentImage
    {
        [Key]
        public long StudentImageId { get; set; }
        public string Data { get; set; }

        public StudentImage(string data)
        {
            Data = data;
        }
    }
}
