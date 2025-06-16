using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Cinema;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Identities;
using Project_PRN222_G5.BusinessLogic.Interfaces.Validation;
using Project_PRN222_G5.BusinessLogic.Services;
using Project_PRN222_G5.BusinessLogic.Services.Cinema;
using Project_PRN222_G5.BusinessLogic.Services.Identities;
using Project_PRN222_G5.BusinessLogic.Validation;
using Project_PRN222_G5.DataAccess.Data;
using Project_PRN222_G5.DataAccess.Interfaces.Data;
using Project_PRN222_G5.DataAccess.Interfaces.Service;
using Project_PRN222_G5.DataAccess.Interfaces.UnitOfWork;
using Project_PRN222_G5.DataAccess.Service;
using Project_PRN222_G5.DataAccess.UnitOfWork;
using Project_PRN222_G5.Web.Utilities;
using System.Text;

namespace Project_PRN222_G5.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        #region HttpContextAccessor

        services.AddHttpContextAccessor();

        #endregion HttpContextAccessor

        #region Service

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICinemaService, CinemaService>();
        services.AddScoped<IJwtService, JwtService>();

        services.AddScoped<IValidationService, ValidationService>();
        services.AddScoped<ITokenValidator, TokenValidator>();
        services.AddScoped<ICookieService, CookieService>();

        services.AddScoped<IStorageService, DiskStorageService>();
        services.AddScoped<IMediaService, MediaService>();

        #endregion Service

        return services;
    }

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        #region DbContext
        services.AddScoped<IDbContext, TheDbContext>();
        services.AddDbContext<TheDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null)));

        #endregion DbContext

        #region UnitOfWork, Service

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
        services.AddSingleton<IDateTimeService, DateTimeService>();

        #endregion UnitOfWork, Service

        return services;
    }

    public static IServiceCollection AddCookieAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        #region Cookie

        services.AddAuthentication("Cookies")
            .AddCookie("Cookies", options =>
            {
                options.LoginPath = PageRoutes.Auth.Login;
                options.AccessDeniedPath = "/Auth/AccessDenied";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(7);
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
            });

        #endregion Cookie

        return services;
    }

    public static IServiceCollection AddCustomLogging(this IServiceCollection services)
    {
        services.AddLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
            logging.AddFilter(nameof(Microsoft), LogLevel.Warning);
        });

        return services;
    }
}