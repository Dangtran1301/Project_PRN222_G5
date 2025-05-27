using Project_PRN222_G5.BusinessLogic.DTOs.Cinema.Request;
using Project_PRN222_G5.BusinessLogic.DTOs.Cinema.Response;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Identities;
using Project_PRN222_G5.BusinessLogic.Interfaces.Validation;
using Project_PRN222_G5.BusinessLogic.Mapper.Cinema;
using Project_PRN222_G5.DataAccess.Entities.Identities.Cinema;
using Project_PRN222_G5.DataAccess.Interfaces.UnitOfWork;

namespace Project_PRN222_G5.BusinessLogic.Services.Identities;

public class CinemaService(
    IUnitOfWork unitOfWork,
    IValidationService validationService
    ) : GenericService<Cinema, CreateCinemaDto, UpdateCinemaDto, CinemaResponse>(unitOfWork, validationService), ICinemaService
{
    protected override CinemaResponse MapToResponse(Cinema entity) => entity.ToCinemaResponse();

    protected override Cinema MapToEntity(CreateCinemaDto request) => request.ToEntity();

    protected override void UpdateEntity(Cinema entity, UpdateCinemaDto request) => entity.UpdateEntity(request);
}