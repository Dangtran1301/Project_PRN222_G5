using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Project_PRN222_G5.DataAccess.Entities.Bookings;
using Project_PRN222_G5.DataAccess.Entities.Cinemas;
using Project_PRN222_G5.DataAccess.Entities.Common;
using Project_PRN222_G5.DataAccess.Entities.Movies;
using Project_PRN222_G5.DataAccess.Entities.Users;
using Project_PRN222_G5.DataAccess.Interfaces.Data;
using Project_PRN222_G5.DataAccess.Interfaces.Service;

namespace Project_PRN222_G5.DataAccess.Data;

public class TheDbContext : DbContext, IDbContext
{
    private readonly IDateTimeService _dateTimeService;
    private readonly IAuthenticatedUserService _authenticatedUserService;

    public TheDbContext(
        DbContextOptions<TheDbContext> options,
        IDateTimeService dateTimeService,
        IAuthenticatedUserService authenticatedUserService
        ) : base(options)
    {
        _dateTimeService = dateTimeService;
        _authenticatedUserService = authenticatedUserService;
    }

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
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        modelBuilder.SeedData();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var auditableEntries = ChangeTracker.Entries<IBaseAuditable>();
        foreach (var entry in auditableEntries)
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
                entity.CreatedAt = _dateTimeService.NowUtc;
                if (entity.CreatedBy == Guid.Empty)
                {
                    entity.CreatedBy = Guid.TryParse(_authenticatedUserService.UserId, out var userId)
                        ? userId
                        : Guid.Empty;
                }
                break;

            case EntityState.Modified:
                entity.UpdatedAt = _dateTimeService.NowUtc;
                entity.UpdatedBy = Guid.TryParse(_authenticatedUserService.UserId, out var updatedBy)
                    ? updatedBy
                    : null;
                break;
        }
    }
}