using Microsoft.EntityFrameworkCore;
using Project_PRN222_G5.DataAccess.Entities.Bookings;
using Project_PRN222_G5.DataAccess.Entities.Cinemas;
using Project_PRN222_G5.DataAccess.Entities.Movies;
using Project_PRN222_G5.DataAccess.Entities.Movies.Enum;
using Project_PRN222_G5.DataAccess.Entities.Users;
using Project_PRN222_G5.DataAccess.Entities.Users.Enum;

namespace Project_PRN222_G5.DataAccess.Data
{
    public static class DataSeeder
    {
        private static readonly DateTimeOffset SeedDate = new(2025, 6, 1, 0, 0, 0, TimeSpan.FromHours(7));

        public static void SeedData(this ModelBuilder modelBuilder)
        {
            modelBuilder.SeedUsers();
            modelBuilder.SeedCinemas();
            modelBuilder.SeedRooms();
            modelBuilder.SeedSeats();
            modelBuilder.SeedMovies();
            modelBuilder.SeedShowtimes();
            modelBuilder.SeedBookings();
            modelBuilder.SeedBookingDetails();
        }

        private static void SeedUsers(this ModelBuilder modelBuilder)
        {
            var adminId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var userId = Guid.Parse("22222222-2222-2222-2222-222222222222");

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = adminId,
                    FullName = "Admin User",
                    Username = "admin",
                    PasswordHash = "$2b$10$2ZgkaUmY6vSIXTFKg7fpkewjufMZkj2brKEmxNyRFhPv3Ih7bOEte",
                    Email = "admin@example.com",
                    PhoneNumber = "0123456789",
                    DayOfBirth = new DateTime(1990, 1, 1),
                    Gender = Gender.Male,
                    Avatar = "/avatars/admin.jpg",
                    UserStatus = UserStatus.Active,
                    Role = Role.Admin,
                    CreatedAt = SeedDate,
                    CreatedBy = adminId
                },
                new User
                {
                    Id = userId,
                    FullName = "John Doe",
                    Username = "johndoe",
                    PasswordHash = "$2b$10$32EfCFLMW5moXFM8dJSP0Ojo37cEfI6rKO7KK5FxFI2jJr2HgakKm",
                    Email = "john.doe@example.com",
                    PhoneNumber = "0987654321",
                    DayOfBirth = new DateTime(1995, 5, 15),
                    Gender = Gender.Male,
                    Avatar = "/avatars/johndoe.jpg",
                    UserStatus = UserStatus.Active,
                    Role = Role.Customer,
                    CreatedAt = SeedDate,
                    CreatedBy = adminId
                }
            );
        }

        private static void SeedCinemas(this ModelBuilder modelBuilder)
        {
            var adminId = Guid.Parse("11111111-1111-1111-1111-111111111111");

            modelBuilder.Entity<Cinema>().HasData(
                new Cinema
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    Name = "Galaxy Cinema",
                    Address = "123 Main Street, Haboi",
                    CreatedAt = SeedDate,
                    CreatedBy = adminId
                },
                new Cinema
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                    Name = "CGV Cinema",
                    Address = "456 Oak Avenue, Ho Tri Linh City",
                    CreatedAt = SeedDate,
                    CreatedBy = adminId
                }
            );
        }

        private static void SeedRooms(this ModelBuilder modelBuilder)
        {
            var adminId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var cinemaId = Guid.Parse("55555555-5555-5555-5555-555555555555");

            modelBuilder.Entity<Room>().HasData(
                new Room
                {
                    Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                    Name = "Room 1",
                    CinemaId = cinemaId,
                    CreatedAt = SeedDate,
                    CreatedBy = adminId
                },
                new Room
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                    Name = "Room 2",
                    CinemaId = cinemaId,
                    CreatedAt = SeedDate,
                    CreatedBy = adminId
                }
            );
        }

        private static void SeedSeats(this ModelBuilder modelBuilder)
        {
            var adminId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var roomId = Guid.Parse("77777777-7777-7777-7777-777777777777");

            modelBuilder.Entity<Seat>().HasData(
                new Seat
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    SeatNumber = "A1",
                    RoomId = roomId,
                    CreatedAt = SeedDate,
                    CreatedBy = adminId
                },
                new Seat
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    SeatNumber = "A2",
                    RoomId = roomId,
                    CreatedAt = SeedDate,
                    CreatedBy = adminId
                },
                new Seat
                {
                    Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    SeatNumber = "B1",
                    RoomId = roomId,
                    CreatedAt = SeedDate,
                    CreatedBy = adminId
                },
                new Seat
                {
                    Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    SeatNumber = "B2",
                    RoomId = roomId,
                    CreatedAt = SeedDate,
                    CreatedBy = adminId
                }
            );
        }

        private static void SeedMovies(this ModelBuilder modelBuilder)
        {
            var adminId = Guid.Parse("11111111-1111-1111-1111-111111111111");

            modelBuilder.Entity<Movie>().HasData(
                new Movie
                {
                    Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                    Title = "Inception",
                    Description = "A thief who steals corporate secrets through dream infiltration technology.",
                    Genre = "Sci-Fi",
                    Duration = 148,
                    PosterPath = "/images/inception.jpg",
                    Status = MovieStatus.Active,
                    CreatedAt = SeedDate,
                    CreatedBy = adminId
                },
                new Movie
                {
                    Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                    Title = "The Dark Knight",
                    Description = "Batman faces the Joker, a criminal mastermind.",
                    Genre = "Action",
                    Duration = 152,
                    PosterPath = "/images/darkknight.jpg",
                    Status = MovieStatus.Active,
                    CreatedAt = SeedDate,
                    CreatedBy = adminId
                }
            );
        }

        private static void SeedShowtimes(this ModelBuilder modelBuilder)
        {
            var adminId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var movieId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd");
            var roomId1 = Guid.Parse("77777777-7777-7777-7777-777777777777");
            var roomId2 = Guid.Parse("88888888-8888-8888-8888-888888888888");

            _ = modelBuilder.Entity<Showtime>().HasData(
                new Showtime
                {
                    Id = 1,
                    MovieId = movieId,
                    RoomId = roomId1,
                    StartTime = SeedDate.AddHours(2),
                    Price = 10.00m,
                    CreatedAt = SeedDate,
                    CreatedBy = adminId
                },
                new Showtime
                {
                    Id = 2,
                    MovieId = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                    RoomId = roomId2,
                    StartTime = SeedDate.AddHours(4),
                    Price = 12.00m,
                    CreatedAt = SeedDate,
                    CreatedBy = adminId
                }
            );
        }

        private static void SeedBookings(this ModelBuilder modelBuilder)
        {
            var adminId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var userId = Guid.Parse("22222222-2222-2222-2222-222222222222");

            _ = modelBuilder.Entity<Booking>().HasData(
                new Booking
                {
                    Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                    UserId = userId,
                    ShowtimeId = 1,
                    BookingTime = SeedDate.DateTime,
                    TotalPrice = 20.00m,
                    CreatedAt = SeedDate,
                    CreatedBy = adminId
                }
            );
        }

        private static void SeedBookingDetails(this ModelBuilder modelBuilder)
        {
            var bookingId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff");
            var seatId1 = Guid.Parse("99999999-9999-9999-9999-999999999999");
            var seatId2 = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

            modelBuilder.Entity<BookingDetail>().HasData(
                new BookingDetail
                {
                    BookingId = bookingId,
                    SeatId = seatId1,
                    Price = 10.00m
                },
                new BookingDetail
                {
                    BookingId = bookingId,
                    SeatId = seatId2,
                    Price = 10.00m
                }
            );
        }
    }
}