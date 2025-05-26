using Project_PRN222_G5.Infrastructure.Entities.Cinema;
using Project_PRN222_G5.Infrastructure.Entities.Common;

namespace Project_PRN222_G5.Infrastructure.Entities.Movie;

public class Showtime : BaseEntity<int>
{
    public Guid MovieId { get; set; }
    public Movie Movie { get; set; } = null!;
    public Guid RoomId { get; set; }
    public Room Room { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public decimal Price { get; set; }
    public ICollection<Booking.Booking> Bookings { get; set; } = [];
}