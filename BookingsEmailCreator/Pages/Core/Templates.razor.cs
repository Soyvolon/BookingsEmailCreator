using BookingsEmailCreator.Data.Emails;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookingsEmailCreator.Pages.Core;

public partial class Templates
{
#pragma warning disable CS8618 // Injections/Cascading values are never null.
    [Inject]
    public IEmailTemplateService EmailTemplateService { get; set; }

    [Inject]
    public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    private List<EmailTemplate> TemplateList { get; set; } = new();

    public EmailTemplate SelectedTemplate { get; private set; }
    public bool IsEditing { get; private set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity?.IsAuthenticated ?? false)
            {
                await ReloadTemplatesAsync();
                StateHasChanged();
            }
        }
    }

    private async Task ReloadTemplatesAsync()
    {
        TemplateList = await EmailTemplateService.GetEmailTemplatesForUserAsync();
    }

    private void OnTemplateChanged(EmailTemplate newTemplate)
    {
        if (!IsEditing)
            SelectedTemplate = newTemplate;
    }

    private void StartEdit()
    {
        if (!IsEditing)
        {
            if (SelectedTemplate is not null)
            {
                IsEditing = true;
                StateHasChanged();
            }
        }
    }

    private void SaveChanges()
    {
        if (IsEditing && SelectedTemplate is not null)
        {

        }
    }
}