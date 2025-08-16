using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using Swim_Feedback.Data;
using Swim_Feedback.Enums;

namespace Swim_Feedback.Pages
{
    public partial class AvatarCustomize : ComponentBase
    {
        [Parameter]
        public long StudentId { get; set; }

        [Inject]
        public IDbContextFactory<ApplicationDbContext>? DbContextFactory { get; set; }

        [Inject]
        public IJSRuntime? JS { get; set; }

        private Student? student;
        private Avatar? avatar;

        private readonly List<BodyPart> skins = new();
        private readonly List<BodyPart> faceForms = new();
        private readonly List<BodyPart> hairs = new();
        private readonly List<BodyPart> eyePairs = new();
        private readonly List<BodyPart> mouths = new();

        private readonly List<Accessory> backgroundAccessories = new();
        private readonly List<Accessory> hairAccessories = new();
        private readonly List<Accessory> eyePairAccessories = new();
        private readonly List<Accessory> mouthAccessories = new();
        private readonly List<Accessory> neckAccessories = new();

        private bool hideBackgrounds = false;
        private bool hideAccessories = true;
        private bool hideBodyParts = true;
        private bool hideSkins = true;

        private void SwitchCategory(string category)
        {
            hideBackgrounds = true;
            hideAccessories = true;
            hideBodyParts = true;
            hideSkins = true;

            switch (category)
            {
                case "backgrounds":
                    hideBackgrounds = false;
                    break;
                case "accessories":
                    hideAccessories = false;
                    break;
                case "bodyParts":
                    hideBodyParts = false;
                    break;
                case "skins":
                    hideSkins = false;
                    break;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            ApplicationDbContext dbContext = await DbContextFactory.CreateDbContextAsync();

            student = await dbContext.Students
                .FirstOrDefaultAsync(s => s.StudentId == StudentId);

            avatar = await dbContext.Avatars
                .Where(a => a.AvatarId == student.AvatarId)
                .Include(a => a.Skin)
                .Include(a => a.FaceForm)
                .Include(a => a.Hair)
                .Include(a => a.Eyes)
                .Include(a => a.Mouth)
                .Include(a => a.BackgroundAccessory)
                .Include(a => a.AvatarAccessories)
                .FirstAsync();

            List<BodyPart> bodyParts = await dbContext.BodyParts.ToListAsync();

            foreach (BodyPart bodyPart in bodyParts)
            {
                switch (bodyPart.BodyPartType)
                {
                    case EBodyPartType.Skin:
                        skins.Add(bodyPart);
                        break;
                    case EBodyPartType.FaceForm:
                        faceForms.Add(bodyPart);
                        break;
                    case EBodyPartType.Hair:
                        hairs.Add(bodyPart);
                        break;
                    case EBodyPartType.EyePair:
                        eyePairs.Add(bodyPart);
                        break;
                    case EBodyPartType.Mouth:
                        mouths.Add(bodyPart);
                        break;
                }
            }

            List<Accessory> accessories = await dbContext.Accessories
                .Where(a => a.Cost == 0)
                .ToListAsync();

            accessories.AddRange(
                await dbContext.AvatarAccessories
                .Where(aa => aa.AvatarId == avatar.AvatarId)
                .Include(aa => aa.Accessory)
                .Select(aa => aa.Accessory)
                .ToListAsync());

            foreach (Accessory accessory in accessories)
            {
                switch (accessory.AccessoryType)
                {
                    case EAccessoryType.Background:
                        backgroundAccessories.Add(accessory);
                        break;
                    case EAccessoryType.Hair:
                        hairAccessories.Add(accessory);
                        break;
                    case EAccessoryType.EyePair:
                        eyePairAccessories.Add(accessory);
                        break;
                    case EAccessoryType.Mouth:
                        mouthAccessories.Add(accessory);
                        break;
                    case EAccessoryType.Neck:
                        neckAccessories.Add(accessory);
                        break;

                }
            }

            await JS.InvokeAsync<string>("createCanvas");
            await JS.InvokeAsync<string>("addCanvasImg", faceForms[0].Image);
        }

        private async Task SelectBodyPart(BodyPart bodyPart)
        {
            await JS.InvokeAsync<string>("addCanvasImg", bodyPart.Image);
        }
    }
}
