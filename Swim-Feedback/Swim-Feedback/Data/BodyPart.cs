using Swim_Feedback.Enums;
using System.ComponentModel.DataAnnotations;

namespace Swim_Feedback.Data
{
    public class BodyPart
    {
        [Key]
        public long BodyPartId { get; set; }
        public EBodyPartType BodyPartType { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }

        public BodyPart(EBodyPartType bodyPartType, string name, string image)
        {
            BodyPartType = bodyPartType;
            Name = name;
            Image = image;
        }
    }
}
