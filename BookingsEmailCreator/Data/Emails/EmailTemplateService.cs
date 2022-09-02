using BookingsEmailCreator.Data.Db;
using Microsoft.EntityFrameworkCore;

namespace BookingsEmailCreator.Data.Emails;

public class EmailTemplateService : IEmailTemplateService
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
    private readonly IAccountService _accountService;

    public EmailTemplateService(IDbContextFactory<ApplicationDbContext> dbContextFactory, IAccountService accountService)
    {
        _dbContextFactory = dbContextFactory;
        _accountService = accountService;
    }

    public async Task<EmailTemplate?> CreateNewTemplateAsync(string name, string contents, Guid userKey)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var template = new EmailTemplate()
        {
            Name = name,
            Tempalte = contents,
            AccountKey = userKey
        };

        var tracker = await dbContext.AddAsync(template);
        await dbContext.SaveChangesAsync();

        // Get the key valule that was generated.
        await tracker.ReloadAsync();

        return template;
    }

    public async Task DeleteEmailTemplateAsync(Guid key)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var item = await dbContext.EmailTemplates
            .Where(x => x.Key == key)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (item is not null)
        {
            dbContext.Remove(item);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<EmailTemplate?> GetEmailTemplateAsync(Guid key)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var item = await dbContext.EmailTemplates
            .Where(x => x.Key == key)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return item;
    }

    public async Task<List<EmailTemplate>> GetEmailTemplatesForUserAsync(Guid userKey)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        return await dbContext.EmailTemplates
            .Where(x => x.AccountKey == userKey)
            .ToListAsync();
    }

    public async Task UpdateEmailTemplateAsync(Guid key, Action<EmailTemplate> update)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        // We don't use the existing get method here because we want to track changes
        // on the DB instance - and that does not carry between mehtods.
        var item = await dbContext.EmailTemplates
            .Where(x => x.Key == key)
            .FirstOrDefaultAsync();

        if (item is not null)
        {
            update.Invoke(item);
            await dbContext.SaveChangesAsync();
        }
    }
}
