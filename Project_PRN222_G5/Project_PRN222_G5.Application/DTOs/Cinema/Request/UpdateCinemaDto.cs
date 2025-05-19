using Project_PRN222_G5.Application.Interfaces;

namespace Project_PRN222_G5.Application.DTOs.Cinema.Request;

public class UpdateCinemaDto : IMapTo<Domain.Entities.Cinema.Cinema>
{
    public required string Name { get; set; } = string.Empty;
    public required string Address { get; set; } = string.Empty;

    public Domain.Entities.Cinema.Cinema ToEntity() => new Domain.Entities.Cinema.Cinema()
    {
        Name = Name,
        Address = Address,
        UpdatedBy = string.Empty,
        UpdatedAt = DateTimeOffset.Now
    };
}