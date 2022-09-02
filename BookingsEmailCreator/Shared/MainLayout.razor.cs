using BookingsEmailCreator.Data.Db;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Identity.Client;

namespace BookingsEmailCreator.Shared;

public partial class MainLayout
{
#pragma warning disable CS8618 // Injections/Cascading values are never null.
    [Inject]
    public IAccountService AccountService { get; set; }

    [Inject]
    public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public Guid UserKey { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity?.IsAuthenticated ?? false)
            {
                try
                {
                    UserKey = await AccountService.EnsureAccountCreatedAsync();
                }
                catch (MsalUiRequiredException)
                {
                    // Route the user to the API to verify a challenge here.

                }

                StateHasChanged();
            }
        }
    }
}
