using Project_PRN222_G5.Domain.Common;

namespace Project_PRN222_G5.Domain.Entities.Cinema;

public class Cinema : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public ICollection<Room> Rooms { get; set; } = [];
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

}