using Microsoft.EntityFrameworkCore;
using Project_PRN222_G5.Domain.Entities.Users;
using Project_PRN222_G5.Domain.Entities.Users.Enum;

namespace Project_PRN222_G5.Infrastructure.Data.Seeder
{
    public class DatabaseSeeder
    {
        public static async Task SeedAsync(TheDbContext context)
        {
            if (!await context.Users.AnyAsync(u => u.Username == "admin"))
            {
                var adminUser = new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Admin",
                    LastName = "User",
                    Username = "admin",
                    Email = "admin@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    Roles = new List<Role> { Role.Admin },
                    UserStatus = UserStatus.Active,
                    CreatedAt = DateTimeOffset.UtcNow,
                    CreatedBy = "System"
                };

                await context.Users.AddAsync(adminUser);
                await context.SaveChangesAsync();
            }
        }
    }
}