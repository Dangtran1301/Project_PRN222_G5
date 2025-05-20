using Project_PRN222_G5.Application.Interfaces.Mapping;

namespace Project_PRN222_G5.Application.DTOs.Cinema.Request;

public class UpdateCinemaDto : IMapTo<Domain.Entities.Cinema.Cinema>
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public Guid Id { get; set; }  

    public Domain.Entities.Cinema.Cinema ToEntity() => new Domain.Entities.Cinema.Cinema()
    {
        Id = Id,
        Name = Name,
        Address = Address,
        UpdatedAt = DateTimeOffset.Now
    };
}