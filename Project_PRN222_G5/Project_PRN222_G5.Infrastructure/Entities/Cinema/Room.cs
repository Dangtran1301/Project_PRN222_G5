using Project_PRN222_G5.Infrastructure.Entities.Common;
using Project_PRN222_G5.Infrastructure.Entities.Movie;

namespace Project_PRN222_G5.Infrastructure.Entities.Cinema;

public class Room : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public Guid CinemaId { get; set; }
    public Cinema Cinema { get; set; } = null!;
    public ICollection<Seat> Seats { get; set; } = [];
    public ICollection<Showtime> Showtimes { get; set; } = [];
}