using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Swim_Feedback.Data;

namespace Swim_Feedback.Pages
{
    public partial class Shop : ComponentBase
    {
        [Parameter]
        public long StudentId { get; set; }

        [Inject]
        public IDbContextFactory<ApplicationDbContext>? DbContextFactory { get; set; }

        private List<Accessory>? accessories;
        private Student? student;
        private Avatar? avatar;

        private async Task Buy(Accessory accessory)
        {
            ApplicationDbContext dbContext = await DbContextFactory.CreateDbContextAsync();

            student.Points -= accessory.Cost;
            avatar.AvatarAccessories.Add(new AvatarAccessory(avatar, accessory));

            dbContext.Students.Update(student);
            await dbContext.SaveChangesAsync();
        }

        protected override async Task OnInitializedAsync()
        {
            ApplicationDbContext dbContext = await DbContextFactory.CreateDbContextAsync();

            accessories = await dbContext.Accessories.ToListAsync();

            student = await dbContext.Students
                .FirstOrDefaultAsync(s => s.StudentId == StudentId);

            avatar = await dbContext.Avatars
                .Include(a => a.AvatarAccessories)
                .FirstOrDefaultAsync(a => a.AvatarId == student.AvatarId);
        }
    }
}
