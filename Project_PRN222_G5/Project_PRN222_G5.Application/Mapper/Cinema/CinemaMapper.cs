using Project_PRN222_G5.Application.DTOs.Cinema.Request;
using Project_PRN222_G5.Application.DTOs.Cinema.Response;

namespace Project_PRN222_G5.Application.Mapper.Cinema;

public static class CinemaMapper
{
    public static CinemaResponse ToCinemaResponse(this Domain.Entities.Cinema.Cinema entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Address = entity.Address,
        CreatedAt = entity.CreatedAt,
        UpdatedAt = entity.UpdatedAt,
    };

    public static void UpdateEntity(this Domain.Entities.Cinema.Cinema entity, UpdateCinemaDto request)
    {
        entity.Name = request.Name;
        entity.Address = request.Address;
        entity.UpdatedAt = DateTimeOffset.UtcNow;
    }
}