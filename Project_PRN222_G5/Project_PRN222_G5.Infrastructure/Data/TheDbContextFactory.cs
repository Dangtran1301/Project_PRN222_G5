using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Project_PRN222_G5.Application.Interfaces.Service;

namespace Project_PRN222_G5.Infrastructure.Data;

public class TheDbContextFactory : IDesignTimeDbContextFactory<TheDbContext>
{
    public TheDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(
                @Directory.GetCurrentDirectory() + "/../Project_PRN222_G5.Web/appsettings.Development.json")
            .Build();

        var connectionString = configuration.GetValue<string>(
            "ConnectionStrings:DefaultConnection"
                );

        var optionsBuilder = new DbContextOptionsBuilder<TheDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        IAuthenticatedUserService authenticatedUserService = new DefaultAuthenticatedUserService();
        IDateTimeService datetimeService = new DefaultDatetimeService();

        return new TheDbContext(optionsBuilder.Options, datetimeService, authenticatedUserService);
    }
}

public class DefaultDatetimeService : IDateTimeService
{
    public DateTime NowUtc => DateTime.MinValue;
}

public class DefaultAuthenticatedUserService : IAuthenticatedUserService
{
    public string UserId => string.Empty;
    public string ClientIp => string.Empty;
}