using Project_PRN222_G5.DataAccess.Entities.Common;
using Project_PRN222_G5.DataAccess.Entities.Identities.Booking;

namespace Project_PRN222_G5.DataAccess.Entities.Identities.Cinema;

public class Seat : BaseEntity
{
    public Guid RoomId { get; set; }
    public Room Room { get; set; } = null!;
    public string SeatNumber { get; set; } = string.Empty;
    public ICollection<BookingDetail> BookingDetails { get; set; } = [];
}