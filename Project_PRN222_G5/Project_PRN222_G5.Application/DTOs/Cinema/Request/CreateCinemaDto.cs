using Project_PRN222_G5.BusinessLogic.Interfaces.Mapping;

namespace Project_PRN222_G5.BusinessLogic.DTOs.Cinema.Request;

public class CreateCinemaDto : IMapTo<DataAccess.Entities.Identities.Cinema.Cinema>
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public DataAccess.Entities.Identities.Cinema.Cinema ToEntity() => new()
    {
        Name = Name,
        Address = Address,
    };
}