using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Cinema;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Identities;
using Project_PRN222_G5.BusinessLogic.Interfaces.Validation;
using Project_PRN222_G5.BusinessLogic.Services.Cinema;
using Project_PRN222_G5.BusinessLogic.Services.Identities;
using Project_PRN222_G5.BusinessLogic.Validation;
using Project_PRN222_G5.DataAccess.Data;
using Project_PRN222_G5.DataAccess.Interfaces.Data;
using Project_PRN222_G5.DataAccess.Interfaces.Service;
using Project_PRN222_G5.DataAccess.Interfaces.UnitOfWork;
using Project_PRN222_G5.DataAccess.Service;
using Project_PRN222_G5.DataAccess.UnitOfWork;
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

        #endregion Service

        return services;
    }

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        #region DbContext

        services.AddDbContext<TheDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null)));
        services.AddScoped<IDbContext, TheDbContext>();

        #endregion DbContext

        #region UnitOfWork, Service

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
        services.AddSingleton<IDateTimeService, DateTimeService>();

        #endregion UnitOfWork, Service

        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration
                    ["Jwt:Key"]!))
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    context.Token = context.Request.Cookies["AccessToken"];
                    return Task.CompletedTask;
                }
            };
            options.MapInboundClaims = false;
        });

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