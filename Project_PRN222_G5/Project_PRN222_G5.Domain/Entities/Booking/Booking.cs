using Project_PRN222_G5.Domain.Common;
using Project_PRN222_G5.Domain.Entities.Movie;
using Project_PRN222_G5.Domain.Entities.Users;

namespace Project_PRN222_G5.Domain.Entities.Booking;

public class Booking : BaseEntity
{
    public DateTime BookingTime { get; set; } = DateTime.Now;
    public decimal TotalPrice { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int ShowtimeId { get; set; }
    public Showtime Showtime { get; set; } = null!;
    public ICollection<BookingDetail> BookingDetails { get; set; } = [];
}