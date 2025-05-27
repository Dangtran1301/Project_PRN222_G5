using Project_PRN222_G5.DataAccess.Entities.Common;
using Project_PRN222_G5.DataAccess.Entities.Identities.Movie;
using Project_PRN222_G5.DataAccess.Entities.Identities.Users;

namespace Project_PRN222_G5.DataAccess.Entities.Identities.Booking;

public class Booking : BaseEntity
{
    public DateTime BookingTime { get; set; } = DateTime.Now;
    public decimal TotalPrice { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public int ShowtimeId { get; set; }
    public Showtime Showtime { get; set; } = null!;
    public ICollection<BookingDetail> BookingDetails { get; set; } = [];
}