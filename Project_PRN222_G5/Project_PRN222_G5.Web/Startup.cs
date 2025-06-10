using Project_PRN222_G5.Web.Middleware;
using Project_PRN222_G5.Web.Utilities;

namespace Project_PRN222_G5.Web;

public class Startup(IConfiguration configuration)
{
    private IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        #region Razor Pages & MVC

        services.AddRazorPages();
        services.AddControllersWithViews();

        #endregion Razor Pages & MVC

        #region Services

        services
            .AddApplicationServices(Configuration)
            .AddInfrastructureServices(Configuration)
            .AddJwtAuthentication(Configuration)
            .AddCustomLogging();

        #endregion Services

        #region Cookie

        services.AddAuthentication("Cookies")
            .AddCookie("Cookies", options =>
            {
                options.LoginPath = PageRoutes.Auth.Login;
                options.AccessDeniedPath = "/Auth/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromDays(7);
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
            });

        #endregion Cookie
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
            app.UseExceptionHandler(PageRoutes.Public.Error);
        }

        app.UseHttpsRedirection();

        app.UseRouting();
        app.UseStaticFiles();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseAuthorizationMiddleware();
        app.UseLoggerMiddleware();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            endpoints.MapRazorPages();
        }
        );
    }
}