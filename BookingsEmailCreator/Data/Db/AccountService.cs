using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;

namespace BookingsEmailCreator.Data.Db;

public class AccountService : IAccountService
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
    private readonly GraphServiceClient _graph;

    public AccountService(IDbContextFactory<ApplicationDbContext> dbContextFactory, GraphServiceClient graph)
    {
        _dbContextFactory = dbContextFactory;
        _graph = graph;
    }

    public async Task<Guid> EnsureAccountCreatedAsync()
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var me = await _graph.Me.Request().GetAsync();

        var user = await dbContext.Accounts
            .Where(x => x.MsId == me.Id)
            .FirstOrDefaultAsync();

        if (user is null)
        {
            user = new AccountData()
            {
                MsId = me.Id
            };

            var tracker = await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();

            await tracker.ReloadAsync();
        }

        return user.Key;
    }
}
