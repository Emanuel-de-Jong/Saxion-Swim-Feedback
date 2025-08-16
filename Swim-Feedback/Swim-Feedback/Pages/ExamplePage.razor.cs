using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using Swim_Feedback.Data;
using Swim_Feedback.Services;
using Swim_Feedback.Services.PageDataService;
using System.Security.Claims;

namespace Swim_Feedback.Pages
{
    public partial class ExamplePage : ComponentBase, IDisposable, IAsyncDisposable
    {
        [Parameter]
        public int PageId { get; set; }

        [Inject]
        public ExampleService? MyExampleService { get; set; }

        [Inject]
        public PageDataService? PageDataService { get; set; }

        [Inject]
        public IJSRuntime? JS { get; set; }

        [Inject]
        public IDbContextFactory<ApplicationDbContext>? DbContextFactory { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState>? authenticationStateTask { get; set; }

        [Inject]
        private UserManager<IdentityUser>? userManager { get; set; }

        private IdentityUser? user;

        private DotNetObjectReference<ExamplePage>? examplePageRef;

        private List<Example>? exampleList;

        private string? exampleName;

        private async Task SaveExample()
        {
            ApplicationDbContext dbContext = DbContextFactory.CreateDbContext();

            await dbContext.Examples.AddAsync(new Example(exampleName));
            await dbContext.SaveChangesAsync();

            exampleList = await dbContext.Examples.ToListAsync();

            StateHasChanged();
        }

        [JSInvokable]
        public void JSToCS(string text)
        {
            Console.WriteLine(text);
        }

        protected override void OnInitialized()
        {
        }

        protected override async Task OnInitializedAsync()
        {
        }

        protected override void OnParametersSet()
        {
        }

        protected override async Task OnParametersSetAsync()
        {
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                ClaimsPrincipal userClaimsPrincipal = (await authenticationStateTask).User;
                if (userClaimsPrincipal.Identity.IsAuthenticated)
                {
                    user = await userManager.GetUserAsync(userClaimsPrincipal);
                }

                examplePageRef = DotNetObjectReference.Create(this);

                MyExampleService.IsTrue();

                ExamplePagePageData? pageData = (ExamplePagePageData)PageDataService.GetData<ExamplePage>();
                if (pageData == null)
                {
                    pageData = new ExamplePagePageData { MyId = new Random().Next(1, 100) };
                    PageDataService.AddData<ExamplePage>(pageData);
                }

                Console.WriteLine(pageData.MyId);

                await JS.InvokeAsync<string>("examplePage.init", examplePageRef);
                await JS.InvokeAsync<string>("examplePage.csToJS", "This is CS");

                ApplicationDbContext dbContext = DbContextFactory.CreateDbContext();
                exampleList = await dbContext.Examples.ToListAsync();

                StateHasChanged();
            }
        }

        public void Dispose()
        {
        }

        public async ValueTask DisposeAsync()
        {
            examplePageRef?.Dispose();
        }
    }
}
