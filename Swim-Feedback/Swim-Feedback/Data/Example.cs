using System.ComponentModel.DataAnnotations;

namespace Swim_Feedback.Data
{
    public class Example
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }


        public Example(string name)
        {
            Name = name;
        }
    }
}
