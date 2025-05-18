using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Project_PRN222_G5.Application.Interfaces;
using Project_PRN222_G5.Application.Services;
using Project_PRN222_G5.Application.Validators;
using Project_PRN222_G5.Domain.Interfaces;
using Project_PRN222_G5.Infrastructure.Data;
using Project_PRN222_G5.Infrastructure.Repositories;
using System.Text;

namespace Project_PRN222_G5.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // 👉 Infrastructure - DbContext
            services.AddDbContext<TheDbContext>(options =>
                options.UseSqlServer(configuration.GetValue<string>("ConnectionStrings:DefaultConnection")));

            services.AddScoped<IDbContext, TheDbContext>();

            // 👉 Infrastructure - UnitOfWork & Repository
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            services.AddScoped(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));

            // 👉 Application - Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            // Thêm các service khác tại đây nếu có

            // 👉 Application - Validators
            services.AddValidatorsFromAssemblyContaining<RegisterUserRequestValidator>();

            // 👉 Logging
            services.AddLogging(logging => logging.AddConsole());

            // 👉 Authentication (JWT)
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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
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
    }
}