using Microsoft.EntityFrameworkCore;

namespace BookingsEmailCreator.Data.Db;

public class ApplicationDbContext : DbContext
{


    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {

    }
}
