using Project_PRN222_G5.Domain.Entities.Cinema;

namespace Project_PRN222_G5.Domain.Entities.Booking;

public class BookingDetail
{
    public Guid BookingId { get; set; }
    public Booking Booking { get; set; } = null!;
    public Guid SeatId { get; set; }
    public Seat Seat { get; set; } = null!;
}