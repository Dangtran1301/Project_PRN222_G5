using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Project_PRN222_G5.Application.Interfaces.Data;
using Project_PRN222_G5.Application.Interfaces.Service;
using Project_PRN222_G5.Domain.Common;
using Project_PRN222_G5.Domain.Entities.Booking;
using Project_PRN222_G5.Domain.Entities.Cinema;
using Project_PRN222_G5.Domain.Entities.Movie;
using Project_PRN222_G5.Domain.Entities.Users;

namespace Project_PRN222_G5.Infrastructure.Data;

public class TheDbContext(
    DbContextOptions<TheDbContext> options,
    IDateTimeService datetimeService,
    IAuthenticatedUserService authenticatedUserService
) : DbContext(options), IDbContext
{
    public DatabaseFacade DatabaseFacade => Database;
    public DbSet<User> Users { get; set; }
    public DbSet<UserToken> UserTokens { get; set; }
    public DbSet<UserResetPassword> UserResetPasswords { get; set; }
    public DbSet<Cinema> Cinemas { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Showtime> Showtimes { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<BookingDetail> BookingDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        foreach (var property in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(t => t.GetProperties())
                     .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetColumnType("decimal(18,6)");
        }

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<IBaseAuditable>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = datetimeService.NowUtc;
                    entry.Entity.CreatedBy ??= authenticatedUserService.UserId ?? "System"; /* if null then equal */
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = datetimeService.NowUtc;
                    entry.Entity.UpdatedBy = authenticatedUserService.UserId;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}