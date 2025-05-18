using Project_PRN222_G5.Domain.Common;
using Project_PRN222_G5.Domain.Entities.Cinema;

namespace Project_PRN222_G5.Domain.Entities.Movie;

public class Showtime : BaseEntity
{
    public int MovieId { get; set; }
    public int RoomId { get; set; }
    public DateTime StartTime { get; set; }
    public decimal Price { get; set; }
    public Movie Movie { get; set; } = null!;
    public Room Room { get; set; } = null!;
    public ICollection<Booking.Booking> Bookings { get; set; } = [];
}