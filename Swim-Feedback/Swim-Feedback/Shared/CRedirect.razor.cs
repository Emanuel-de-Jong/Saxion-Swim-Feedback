using Microsoft.AspNetCore.Components;

namespace Swim_Feedback.Shared
{
    public partial class CRedirect : ComponentBase
    {
        [Parameter]
        public string? URI { get; set; }

        [Inject]
        private NavigationManager? navigationManager { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            navigationManager.NavigateTo("/" + URI);
        }
    }
}
