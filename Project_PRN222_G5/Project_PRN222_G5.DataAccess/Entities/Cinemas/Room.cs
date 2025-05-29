using Project_PRN222_G5.DataAccess.Entities.Common;
using Project_PRN222_G5.DataAccess.Entities.Movies;

namespace Project_PRN222_G5.DataAccess.Entities.Cinemas;

public class Room : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public Guid CinemaId { get; set; }
    public Cinema Cinema { get; set; } = null!;
    public ICollection<Seat> Seats { get; set; } = [];
    public ICollection<Showtime> Showtimes { get; set; } = [];
}