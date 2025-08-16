using Swim_Feedback.Enums;
using System.ComponentModel.DataAnnotations;

namespace Swim_Feedback.Data
{
    public class Accessory
    {
        [Key]
        public long AccessoryId { get; set; }
        public ICollection<AvatarAccessory>? AvatarAccessories { get; set; }
        public EAccessoryType AccessoryType { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public string Image { get; set; }

        public Accessory(EAccessoryType accessoryType, string name, int cost, string image)
        {
            AccessoryType = accessoryType;
            Name = name;
            Cost = cost;
            Image = image;
        }
    }
}
