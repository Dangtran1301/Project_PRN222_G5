using Project_PRN222_G5.Domain.Common;
using Project_PRN222_G5.Domain.Entities.Movie.Enum;

namespace Project_PRN222_G5.Domain.Entities.Movie;

public class Movie : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Genre { get; set; }
    public int? Duration { get; set; }
    public string? PosterPath { get; set; }
    public MovieStatus Status { get; set; } = MovieStatus.Active;
    public ICollection<Showtime> Showtimes { get; set; } = [];
}