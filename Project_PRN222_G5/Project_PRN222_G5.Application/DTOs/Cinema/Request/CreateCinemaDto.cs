using System.ComponentModel.DataAnnotations;
using Project_PRN222_G5.Application.Interfaces.Mapping;

namespace Project_PRN222_G5.Application.DTOs.Cinema.Request;

public class CreateCinemaDto : IMapTo<Domain.Entities.Cinema.Cinema>
{
    [Required(ErrorMessage = "Cinema name is required.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Cinema name must be between 3 and 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Address is required.")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 200 characters.")]
    public string Address { get; set; } = string.Empty;

    public Domain.Entities.Cinema.Cinema ToEntity() => new()
    {
        Name = Name,
        Address = Address,
        CreatedAt = DateTimeOffset.UtcNow,
    };
}
