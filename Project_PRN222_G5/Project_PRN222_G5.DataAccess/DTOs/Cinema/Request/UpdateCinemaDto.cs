using Project_PRN222_G5.DataAccess.Interfaces.Mapping;
using System.ComponentModel.DataAnnotations;

namespace Project_PRN222_G5.DataAccess.DTOs.Cinema.Request
{
    public class UpdateCinemaDto : IMapTo<Project_PRN222_G5.DataAccess.Entities.Cinemas.Cinema>
    {
        [Required(ErrorMessage = "Cinema name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Cinema name must be between 3 and 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 200 characters.")]
        public string Address { get; set; } = string.Empty;

        public Guid Id { get; set; }

        public Project_PRN222_G5.DataAccess.Entities.Cinemas.Cinema ToEntity() => new()
        {
            Id = Id,
            Name = Name,
            Address = Address,
        };
    }
}