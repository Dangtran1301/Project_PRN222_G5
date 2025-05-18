using Microsoft.Extensions.DependencyInjection;
using Project_PRN222_G5.Domain.Entities.Users;
using Project_PRN222_G5.Domain.Entities.Users.Enum;
using Project_PRN222_G5.Domain.Interfaces;

namespace Project_PRN222_G5.Infrastructure.Data.Seeder
{
    public class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider provider)
        {
            IUnitOfWork unitOfWork = provider.GetRequiredService<IUnitOfWork>();
            if (!await unitOfWork.Repository<User>().AnyAsync(x => x.Username == "admin"))
            {
                var adminUser = new User
                {
                    Id = Guid.NewGuid(),
                    FullName = "Admin",
                    Username = "admin",
                    Email = "admin@example.com",
                    Phone = "666",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    Role = Role.Admin,
                    UserStatus = UserStatus.Active,
                    CreatedAt = DateTimeOffset.UtcNow,
                    CreatedBy = "System"
                };

                await unitOfWork.Repository<User>().AddAsync(adminUser);
                await unitOfWork.CompleteAsync();
            }
        }
    }
}