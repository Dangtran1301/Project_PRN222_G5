using Project_PRN222_G5.BusinessLogic.DTOs.Cinema.Request;
using Project_PRN222_G5.BusinessLogic.DTOs.Cinema.Response;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Cinema;
using Project_PRN222_G5.BusinessLogic.Interfaces.Validation;
using Project_PRN222_G5.BusinessLogic.Mapper.Cinema;
using Project_PRN222_G5.DataAccess.Interfaces.UnitOfWork;

namespace Project_PRN222_G5.BusinessLogic.Services.Cinema;

public class CinemaService(
    IUnitOfWork unitOfWork,
    IValidationService validationService
    ) : GenericService<DataAccess.Entities.Cinemas.Cinema, CreateCinemaDto, UpdateCinemaDto, CinemaResponse>(unitOfWork, validationService), ICinemaService
{
    protected override CinemaResponse MapToResponse(DataAccess.Entities.Cinemas.Cinema entity) => entity.ToCinemaResponse();

    protected override DataAccess.Entities.Cinemas.Cinema MapToEntity(CreateCinemaDto request) => request.ToEntity();

    protected override void UpdateEntity(DataAccess.Entities.Cinemas.Cinema entity, UpdateCinemaDto request) => entity.UpdateEntity(request);
}