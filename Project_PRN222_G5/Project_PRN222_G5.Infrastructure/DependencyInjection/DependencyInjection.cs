using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Project_PRN222_G5.Application.Interfaces.Data;
using Project_PRN222_G5.Application.Interfaces.Repository;
using Project_PRN222_G5.Application.Interfaces.Service;
using Project_PRN222_G5.Application.Interfaces.Service.Identities;
using Project_PRN222_G5.Application.Interfaces.UnitOfWork;
using Project_PRN222_G5.Application.Interfaces.Validation;
using Project_PRN222_G5.Application.Services;
using Project_PRN222_G5.Application.Services.Identities;
using Project_PRN222_G5.Infrastructure.Data;
using Project_PRN222_G5.Infrastructure.Repositories;
using Project_PRN222_G5.Infrastructure.Service;
using System.Text;

namespace Project_PRN222_G5.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            // Application - Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICinemaService, CinemaService>();

            // Application - Validation
            services.AddScoped<IValidationService, ValidationService>();

            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Infrastructure - DbContext
            services.AddDbContext<TheDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IDbContext, TheDbContext>();

            // Infrastructure - Repository & Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            services.AddScoped(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddScoped<IDateTimeService, DateTimeService>();
            services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
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
            });

            return services;
        }

        public static IServiceCollection AddCustomLogging(this IServiceCollection services)
        {
            services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            });

            return services;
        }
    }
}