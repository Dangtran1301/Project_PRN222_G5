using Project_PRN222_G5.DataAccess.Entities.Cinemas;

namespace Project_PRN222_G5.DataAccess.Entities.Bookings;

public class BookingDetail
{
    public Guid BookingId { get; set; }
    public Booking Booking { get; set; } = null!;
    public Guid SeatId { get; set; }
    public Seat Seat { get; set; } = null!;
    public decimal Price { get; set; }
}