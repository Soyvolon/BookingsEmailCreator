namespace BookingsEmailCreator.Data.Db;

public interface IAccountService
{
    public Task<Guid> EnsureAccountCreatedAsync();
}
