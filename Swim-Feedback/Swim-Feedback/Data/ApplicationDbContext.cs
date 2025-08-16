using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Swim_Feedback.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Example> Examples { get; set; }
        public DbSet<Accessory> Accessories { get; set; }
        public DbSet<AdminLog> AdminLogs { get; set; }
        public DbSet<Avatar> Avatars { get; set; }
        public DbSet<BodyPart> BodyParts { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<AvatarAccessory> AvatarAccessories { get; set; }
        public DbSet<StudentFeedback> StudentFeedbacks { get; set; }
        public DbSet<StudentImage> StudentImages { get; set; }
        public DbSet<SwimClass> SwimClasses { get; set; }
        public DbSet<TeacherFeedback> TeacherFeedbacks { get; set; }
        public DbSet<Account> Accounts { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AvatarAccessory>()
                .HasKey(e => new { e.AvatarId, e.AccessoryId });

            builder.Entity<AvatarAccessory>()
                .HasOne(aa => aa.Avatar)
                .WithMany(a => a.AvatarAccessories)
                .HasForeignKey(aa => aa.AvatarId);

            builder.Entity<AvatarAccessory>()
                .HasOne(aa => aa.Accessory)
                .WithMany(a => a.AvatarAccessories)
                .HasForeignKey(aa => aa.AccessoryId);


            builder.Entity<AdminLog>()
                .HasOne(e => e.Giver)
                .WithMany()
                .HasForeignKey(e => e.GiverId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<AdminLog>()
                .HasOne(e => e.Receiver)
                .WithMany()
                .HasForeignKey(e => e.ReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull);


            builder.Entity<Student>()
                .HasOne(e => e.StudentImage)
                .WithMany()
                .HasForeignKey(e => e.StudentImageId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Student>()
                .HasOne(e => e.SwimClass)
                .WithMany()
                .HasForeignKey(e => e.SwimClassId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Student>()
                .HasOne(e => e.Avatar)
                .WithMany()
                .HasForeignKey(e => e.AvatarId)
                .OnDelete(DeleteBehavior.ClientSetNull);


            builder.Entity<StudentFeedback>()
                .HasOne(e => e.Student)
                .WithMany()
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull);


            builder.Entity<TeacherFeedback>()
                .HasOne(e => e.Account)
                .WithMany()
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<TeacherFeedback>()
                .HasOne(e => e.SwimClass)
                .WithMany()
                .HasForeignKey(e => e.SwimClassId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<TeacherFeedback>()
                .HasOne(e => e.Student)
                .WithMany()
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull);


            builder.Entity<Avatar>()
                .HasOne(e => e.Skin)
                .WithMany()
                .HasForeignKey(e => e.SkinId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Avatar>()
                .HasOne(e => e.FaceForm)
                .WithMany()
                .HasForeignKey(e => e.FaceFormId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Avatar>()
                .HasOne(e => e.Hair)
                .WithMany()
                .HasForeignKey(e => e.HairId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Avatar>()
                .HasOne(e => e.Eyes)
                .WithMany()
                .HasForeignKey(e => e.EyesId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Avatar>()
                .HasOne(e => e.Mouth)
                .WithMany()
                .HasForeignKey(e => e.MouthId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Avatar>()
                .HasOne(e => e.BackgroundAccessory)
                .WithMany()
                .HasForeignKey(e => e.BackgroundAccessoryId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Avatar>()
                .HasOne(e => e.HairAccessory)
                .WithMany()
                .HasForeignKey(e => e.HairAccessoryId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Avatar>()
                .HasOne(e => e.EyesAccessory)
                .WithMany()
                .HasForeignKey(e => e.EyesAccessoryId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Avatar>()
                .HasOne(e => e.MouthAccessory)
                .WithMany()
                .HasForeignKey(e => e.MouthAccessoryId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Avatar>()
                .HasOne(e => e.NeckAccessory)
                .WithMany()
                .HasForeignKey(e => e.NeckAccessoryId)
                .OnDelete(DeleteBehavior.ClientSetNull);


            base.OnModelCreating(builder);
        }
    }
}