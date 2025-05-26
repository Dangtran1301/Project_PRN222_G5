using Project_PRN222_G5.Infrastructure.DTOs.Cinema.Request;
using Project_PRN222_G5.Infrastructure.DTOs.Cinema.Response;

namespace Project_PRN222_G5.Application.Mapper.Cinema;

public static class CinemaMapper
{
    public static CinemaResponse ToCinemaResponse(this Infrastructure.Entities.Cinema.Cinema entity) => new()
    {
        Address = entity.Address,
        Name = entity.Name
    };

    public static void UpdateEntity(this Infrastructure.Entities.Cinema.Cinema entity, UpdateCinemaDto request)
    {
        var updateEntity = request.ToEntity();
        entity.Name = updateEntity.Name;
        entity.Address = updateEntity.Address;
    }
}