using Project_PRN222_G5.BusinessLogic.DTOs.Cinema.Request;
using Project_PRN222_G5.BusinessLogic.DTOs.Cinema.Response;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Cinema;
using Project_PRN222_G5.BusinessLogic.Interfaces.Validation;
using Project_PRN222_G5.BusinessLogic.Mapper.Cinema;
using Project_PRN222_G5.DataAccess.Interfaces.UnitOfWork;
using System.Linq.Expressions;

namespace Project_PRN222_G5.BusinessLogic.Services.Cinema;

public class CinemaService(
    IUnitOfWork unitOfWork,
    IValidationService validationService
    ) : GenericService<DataAccess.Entities.Cinemas.Cinema, CreateCinemaDto, UpdateCinemaDto, CinemaResponse>(unitOfWork, validationService), ICinemaService
{
    public override CinemaResponse MapToResponse(DataAccess.Entities.Cinemas.Cinema entity) => entity.ToCinemaResponse();

    public override DataAccess.Entities.Cinemas.Cinema MapToEntity(CreateCinemaDto request) => request.ToEntity();

    public override void UpdateEntity(DataAccess.Entities.Cinemas.Cinema entity, UpdateCinemaDto request) => entity.UpdateEntity(request);

    protected override Expression<Func<DataAccess.Entities.Cinemas.Cinema, string>>[] GetSearchFields() =>
        [
            x=>x.Address,
            x=>x.Name
        ];
}