using Project_PRN222_G5.Application.Interfaces;

namespace Project_PRN222_G5.Application.DTOs.Cinema.Request;

public class CreateCinemaDto : IMapTo<Domain.Entities.Cinema.Cinema>
{
    public required string Name { get; set; } = string.Empty;
    public required string Address { get; set; } = string.Empty;

    public Domain.Entities.Cinema.Cinema ToEntity() => new()
    {
        Name = Name,
        Address = Address,
        CreatedAt = DateTimeOffset.UtcNow,
        CreatedBy = ""
    };
}