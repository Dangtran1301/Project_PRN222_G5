using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Project_PRN222_G5.Application.Interfaces.Data;
using Project_PRN222_G5.Domain.Entities.Booking;
using Project_PRN222_G5.Domain.Entities.Cinema;
using Project_PRN222_G5.Domain.Entities.Movie;
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
    public DbSet<Cinema> Cinemas { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Showtime> Showtimes { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<BookingDetail> BookingDetails { get; set; }

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