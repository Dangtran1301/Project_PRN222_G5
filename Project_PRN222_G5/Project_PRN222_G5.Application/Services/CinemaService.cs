using Project_PRN222_G5.Application.DTOs.Cinema.Request;
using Project_PRN222_G5.Application.DTOs.Cinema.Response;
using Project_PRN222_G5.Application.Interfaces.Service;
using Project_PRN222_G5.Application.Mapper.Cinema;
using Project_PRN222_G5.Domain.Entities.Cinema;
using Project_PRN222_G5.Domain.Interfaces;

namespace Project_PRN222_G5.Application.Services;

public class CinemaService(IUnitOfWork unitOfWork)
    : GenericService<Cinema, CreateCinemaDto, UpdateCinemaDto, CinemaResponse>(unitOfWork), ICinemaService
{
    protected override CinemaResponse MapToResponse(Cinema entity) => entity.ToCinemaResponse();

    protected override Cinema MapToEntity(CreateCinemaDto request) => request.ToEntity();

    protected override void UpdateEntity(Cinema entity, UpdateCinemaDto request) => entity.UpdateEntity(request);
}