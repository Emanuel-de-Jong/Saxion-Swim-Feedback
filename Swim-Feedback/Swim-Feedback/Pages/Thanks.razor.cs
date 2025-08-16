using Microsoft.AspNetCore.Components;

namespace Swim_Feedback.Pages
{
    public partial class Thanks : ComponentBase
    {
        [Parameter]
        public long SwimClassId { get; set; }
        [Parameter]
        public string? EncodedTopic { get; set; }
    }
}
