using Project_PRN222_G5.Domain.Common;
using Project_PRN222_G5.Domain.Entities.Booking;

namespace Project_PRN222_G5.Domain.Entities.Cinema;

public class Seat : BaseEntity
{
    public int RoomId { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public Room Room { get; set; } = null!;
    public ICollection<BookingDetail> BookingDetails { get; set; } = [];
}