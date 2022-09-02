namespace BookingsEmailCreator.Data.Emails;

public interface IEmailTemplateService
{
    public Task<List<EmailTemplate>> GetEmailTemplatesForUserAsync(Guid userKey);
    public Task<EmailTemplate?> GetEmailTemplateAsync(Guid key);
    public Task UpdateEmailTemplateAsync(Guid key, Action<EmailTemplate> update);
    public Task DeleteEmailTemplateAsync(Guid key);
    public Task<EmailTemplate> CreateNewTemplateAsync(string name, string contents, Guid userKey);
}
