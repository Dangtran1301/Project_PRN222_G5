using Project_PRN222_G5.Application.DTOs.Cinema.Request;
using Project_PRN222_G5.Application.DTOs.Cinema.Response;
using Project_PRN222_G5.Application.Interfaces.Service.Identities;
using Project_PRN222_G5.Application.Interfaces.UnitOfWork;
using Project_PRN222_G5.Application.Interfaces.Validation;
using Project_PRN222_G5.Application.Mapper.Cinema;
using Project_PRN222_G5.Domain.Entities.Cinema;

namespace Project_PRN222_G5.Application.Services.Identities;

public class CinemaService(
    IUnitOfWork unitOfWork,
    IValidationService validationService
    ) : GenericService<Cinema, CreateCinemaDto, UpdateCinemaDto, CinemaResponse>(unitOfWork, validationService), ICinemaService
{
    protected override CinemaResponse MapToResponse(Cinema entity) => entity.ToCinemaResponse();

    protected override Cinema MapToEntity(CreateCinemaDto request) => request.ToEntity();

    protected override void UpdateEntity(Cinema entity, UpdateCinemaDto request) => entity.UpdateEntity(request);
}