using Project_PRN222_G5.Infrastructure.Interfaces.Mapping;

namespace Project_PRN222_G5.Infrastructure.DTOs.Cinema.Request;

public class CreateCinemaDto : IMapTo<Entities.Cinema.Cinema>
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public Entities.Cinema.Cinema ToEntity() => new()
    {
        Name = Name,
        Address = Address,
    };
}