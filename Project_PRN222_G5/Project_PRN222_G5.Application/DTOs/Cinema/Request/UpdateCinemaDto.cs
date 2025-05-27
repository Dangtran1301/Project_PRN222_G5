using Project_PRN222_G5.BusinessLogic.Interfaces.Mapping;

namespace Project_PRN222_G5.BusinessLogic.DTOs.Cinema.Request;

public class UpdateCinemaDto : IMapTo<Infrastructure.Entities.Cinema.Cinema>
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public Infrastructure.Entities.Cinema.Cinema ToEntity() => new()
    {
        Name = Name,
        Address = Address,
    };
}