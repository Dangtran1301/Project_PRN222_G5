using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Project_PRN222_G5.Infrastructure.Entities.Booking;
using Project_PRN222_G5.Infrastructure.Entities.Cinema;
using Project_PRN222_G5.Infrastructure.Entities.Common;
using Project_PRN222_G5.Infrastructure.Entities.Movie;
using Project_PRN222_G5.Infrastructure.Entities.Users;
using Project_PRN222_G5.Infrastructure.Interfaces.Data;
using Project_PRN222_G5.Infrastructure.Interfaces.Service;

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
            ApplyAudit(entry.Entity, entry.State);
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAudit(IBaseAuditable entity, EntityState state)
    {
        switch (state)
        {
            case EntityState.Added:
                entity.CreatedAt = datetimeService.NowUtc;
                if (entity.CreatedBy == Guid.Empty)
                {
                    entity.CreatedBy = Guid.TryParse(authenticatedUserService.UserId, out var userId)
                        ? userId
                        : Guid.Empty;
                }
                break;

            case EntityState.Modified:
                entity.UpdatedAt = datetimeService.NowUtc;
                entity.UpdatedBy = Guid.TryParse(authenticatedUserService.UserId, out var updatedBy)
                    ? updatedBy
                    : null;
                break;
        }
    }
}