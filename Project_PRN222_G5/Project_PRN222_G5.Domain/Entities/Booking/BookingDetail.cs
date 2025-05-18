using Project_PRN222_G5.Domain.Common;
using Project_PRN222_G5.Domain.Entities.Cinema;

namespace Project_PRN222_G5.Domain.Entities.Booking;

public class BookingDetail : BaseEntity
{
    public int BookingId { get; set; }
    public int SeatId { get; set; }
    public Booking Booking { get; set; } = null!;
    public Seat Seat { get; set; } = null!;
}