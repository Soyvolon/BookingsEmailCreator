using BookingsEmailCreator.Data.Emails;
using Microsoft.EntityFrameworkCore;

namespace BookingsEmailCreator.Data.Db;

public class ApplicationDbContext : DbContext
{
    public DbSet<AccountData> Accounts { get; internal set; }
    public DbSet<EmailTemplate> EmailTemplates { get; internal set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        var accounts = builder.Entity<AccountData>();
        accounts.HasKey(e => e.Key);

        var emailTemplate = builder.Entity<EmailTemplate>();
        emailTemplate.HasKey(e => e.Key);
        emailTemplate.HasOne(e => e.Account)
            .WithMany(p => p.EmailTemplates)
            .HasForeignKey(e => e.AccountKey);
    }
}
