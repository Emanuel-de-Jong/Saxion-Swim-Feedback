using System.ComponentModel.DataAnnotations;

namespace Swim_Feedback.Data
{
    public class SwimClass
    {
        [Key]
        public long SwimClassId { get; set; }
        public string Name { get; set; }

        public SwimClass(string name)
        {
            Name = name;
        }
    }
}
