using Project_PRN222_G5.Infrastructure.DependencyInjection;
using Project_PRN222_G5.Web.Middleware;

namespace Project_PRN222_G5.Web;

public class Startup(IConfiguration configuration)
{
    private IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        // Add Razor Pages
        services.AddRazorPages();

        // Add services
        services
            .AddApplicationServices(Configuration)
            .AddInfrastructureServices(Configuration)
            .AddJwtAuthentication(Configuration)
            .AddCustomLogging();

        // Add Session
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(60);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.Cookie.SameSite = SameSiteMode.Strict;
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseSession();
        app.UseMiddleware<RequestLoggingMiddleware>();
        app.UseMiddleware<ValidationExceptionMiddleware>();
        app.UseMiddleware<GlobalExceptionMiddleware>();

        app.UseRouting();
        app.UseStaticFiles();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<AccessTokenValidationMiddleware>();
        app.UseMiddleware<AuthorizationMiddleware>();

        app.UseEndpoints(endpoints => endpoints.MapRazorPages());
    }
}