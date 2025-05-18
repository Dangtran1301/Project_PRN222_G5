using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Project_PRN222_G5.Infrastructure.Data;

public class TheDbContextFactory : IDesignTimeDbContextFactory<TheDbContext>
{
    public TheDbContext CreateDbContext(string[] args)
    {
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(
                    @Directory.GetCurrentDirectory()+$"/../Project_PRN222_G5.Web/appsettings.json")
                .Build();

            var connectionString = configuration.GetValue<string>(
                "ConnectionStrings:DefaultConnection"
                    );

            var optionsBuilder = new DbContextOptionsBuilder<TheDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new TheDbContext(optionsBuilder.Options);
        }
    }
}