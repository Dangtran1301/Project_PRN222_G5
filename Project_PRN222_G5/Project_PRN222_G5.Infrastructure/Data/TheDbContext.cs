using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Project_PRN222_G5.Application.Interfaces;
using Project_PRN222_G5.Domain.Entities.Users;
using System.Reflection;

namespace Project_PRN222_G5.Infrastructure.Data;

public class TheDbContext(DbContextOptions<TheDbContext> options)
    : DbContext(options), IDbContext
{

    public DatabaseFacade DatabaseFacade => Database;
    public DbSet<User> Users { get; set; }
    public DbSet<UserToken> UserTokens { get; set; }
    public DbSet<UserResetPassword> UserResetPasswords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    public override int SaveChanges() => base.SaveChanges();

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => base.SaveChangesAsync(cancellationToken);
}
