using Project_PRN222_G5.DataAccess.Entities.Bookings;
using Project_PRN222_G5.DataAccess.Entities.Common;

namespace Project_PRN222_G5.DataAccess.Entities.Cinemas;

public class Seat : BaseEntity
{
    public Guid RoomId { get; set; }
    public Room Room { get; set; } = null!;
    public string SeatNumber { get; set; } = string.Empty;
    public ICollection<BookingDetail> BookingDetails { get; set; } = [];
}