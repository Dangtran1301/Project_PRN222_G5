using Project_PRN222_G5.Web.Middleware;
using Project_PRN222_G5.Web.Utilities;
using System.Threading.RateLimiting;

namespace Project_PRN222_G5.Web;

public class Startup(IConfiguration configuration)
{
    private IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        #region Razor Pages & MVC

        services.AddRazorPages();
        services.AddControllersWithViews();

        services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 10,
                        Window = TimeSpan.FromMinutes(1),
                        QueueLimit = 0,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                    }));

            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });

        #endregion Razor Pages & MVC

        #region Services

        services
            .AddApplicationServices(Configuration)
            .AddInfrastructureServices(Configuration)
            .AddCookieAuthentication(Configuration)
            .AddCustomLogging();

        #endregion Services
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
        app.UseRateLimiter();
        app.UseHttpsRedirection();
        app.UseMiddleware<RequestTimeoutMiddleware>(TimeSpan.FromSeconds(10));
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