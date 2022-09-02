using BookingsEmailCreator.Data.Db;
using BookingsEmailCreator.Data.Emails;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

namespace BookingsEmailCreator;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
        #region Database Setup
        services.AddDbContextFactory<ApplicationDbContext>(options =>
            options.UseSqlite(
                Configuration.GetConnectionString("database"))
#if DEBUG
    .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
#endif
            // Warnings for loading multiple collections without splitting the query.
            // We want this behaviour for accurate data loading (and our lists are not
            // of any large size for the roster) so we are ignoring these.
            .ConfigureWarnings(w =>
                w.Ignore(RelationalEventId.MultipleCollectionIncludeWarning)
            ), ServiceLifetime.Singleton);

        services.AddScoped(p
            => p.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext());
        #endregion

        // Add services to the container.
        services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAd"))
            .EnableTokenAcquisitionToCallDownstreamApi(Configuration.GetValue<string[]>("DownstreamApi:Scopes"))
            .AddMicrosoftGraph(Configuration.GetSection("DownstreamApi"))
            .AddInMemoryTokenCaches();

        services.AddControllersWithViews()
            .AddMicrosoftIdentityUI();

        services.AddAuthorization(options =>
        {
            // By default, all incoming requests will be authorized according to the default policy
            // options.FallbackPolicy = options.DefaultPolicy;
        });

        services.AddScoped<IAccountService, AccountService>()
            .AddScoped<IEmailTemplateService, EmailTemplateService>();

        services.AddRazorPages();
        services.AddServerSideBlazor()
            .AddMicrosoftIdentityConsentHandler();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        
        // Execute Migrations
        app.UseMigrationsEndPoint();

        var factory = app.ApplicationServices.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
        using (var dbContext = factory.CreateDbContext())
            ApplyDatabaseMigrations(dbContext);

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(options =>
        {
            options.MapControllers();
            options.MapBlazorHub();
            options.MapFallbackToPage("/_Host");
        });
    }

    private static void ApplyDatabaseMigrations(DbContext database)
    {
        if (!(database.Database.GetPendingMigrations()).Any())
        {
            return;
        }

        database.Database.Migrate();
        database.SaveChanges();
    }
}