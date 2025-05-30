using Project_PRN222_G5.BusinessLogic.DTOs.Cinema.Request;
using Project_PRN222_G5.BusinessLogic.DTOs.Cinema.Response;

namespace Project_PRN222_G5.BusinessLogic.Mapper.Cinema;

public static class CinemaMapper
{
    public static CinemaResponse ToCinemaResponse(this DataAccess.Entities.Cinemas.Cinema entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Address = entity.Address,
    };

    public static void UpdateEntity(this DataAccess.Entities.Cinemas.Cinema entity, UpdateCinemaDto request)
    {
        var updateEntity = request.ToEntity();
        entity.Name = updateEntity.Name;
        entity.Address = updateEntity.Address;
    }
}