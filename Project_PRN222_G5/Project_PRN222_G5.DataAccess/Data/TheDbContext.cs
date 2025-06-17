using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Project_PRN222_G5.DataAccess.Entities.Bookings;
using Project_PRN222_G5.DataAccess.Entities.Cinemas;
using Project_PRN222_G5.DataAccess.Entities.Movies;
using Project_PRN222_G5.DataAccess.Entities.Users;
using Project_PRN222_G5.DataAccess.Interfaces.Data;

namespace Project_PRN222_G5.DataAccess.Data;

public class TheDbContext : DbContext, IDbContext
{
    public TheDbContext(
        DbContextOptions<TheDbContext> options
        ) : base(options)
    {
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
}