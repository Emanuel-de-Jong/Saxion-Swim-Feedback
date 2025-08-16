using System.ComponentModel.DataAnnotations;

namespace Swim_Feedback.Data
{
    public class Avatar
    {
        [Key]
        public long AvatarId { get; set; }

        public long SkinId { get; set; } = 1;
        public BodyPart Skin { get; set; }
        public long FaceFormId { get; set; } = 1;
        public BodyPart FaceForm { get; set; }
        public long HairId { get; set; } = 1;
        public BodyPart Hair { get; set; }
        public long EyesId { get; set; } = 1;
        public BodyPart Eyes { get; set; }
        public long MouthId { get; set; } = 1;
        public BodyPart Mouth { get; set; }

        public long BackgroundAccessoryId { get; set; } = 1;
        public Accessory BackgroundAccessory { get; set; }
        public long? HairAccessoryId { get; set; }
        public Accessory? HairAccessory { get; set; }
        public long? EyesAccessoryId { get; set; }
        public Accessory? EyesAccessory { get; set; }
        public long? MouthAccessoryId { get; set; }
        public Accessory? MouthAccessory { get; set; }
        public long? NeckAccessoryId { get; set; }
        public Accessory? NeckAccessory { get; set; }

        public ICollection<AvatarAccessory> AvatarAccessories { get; set; } = new List<AvatarAccessory>();

        public Avatar() { }

        public Avatar(Avatar avatar)
        {
            SkinId = avatar.SkinId;
            FaceFormId = avatar.FaceFormId;
            HairId = avatar.HairId;
            EyesId = avatar.EyesId;
            MouthId = avatar.MouthId;

            BackgroundAccessoryId = avatar.BackgroundAccessoryId;
            HairAccessoryId = avatar.HairAccessoryId;
            EyesAccessoryId = avatar.EyesAccessoryId;
            MouthAccessoryId = avatar.MouthAccessoryId;
            NeckAccessoryId = avatar.NeckAccessoryId;
        }
    }
}
