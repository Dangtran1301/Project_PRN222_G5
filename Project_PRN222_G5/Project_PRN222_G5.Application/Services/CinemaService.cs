using Project_PRN222_G5.Application.DTOs.Cinema.Request;
using Project_PRN222_G5.Application.DTOs.Cinema.Response;
using Project_PRN222_G5.Application.Interfaces.Service;
using Project_PRN222_G5.Application.Mapper.Cinema;
using Project_PRN222_G5.Domain.Entities.Cinema;

using Project_PRN222_G5.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;


namespace Project_PRN222_G5.Application.Services;

public class CinemaService(IUnitOfWork unitOfWork)
    : GenericService<Cinema, CreateCinemaDto, UpdateCinemaDto, CinemaResponse>(unitOfWork), ICinemaService
{
    protected override CinemaResponse MapToResponse(Cinema entity) => entity.ToCinemaResponse();

    protected override Cinema MapToEntity(CreateCinemaDto request) => request.ToEntity();

    protected override void UpdateEntity(Cinema entity, UpdateCinemaDto request) => entity.UpdateEntity(request);

    //Check Duplicate
    public async Task ValidateUniqueCinemaAsync(string name, Guid? excludingId = null)
    {
        var repo = unitOfWork.Repository<Cinema>();

        bool exists;
        if (excludingId.HasValue)
        {
            exists = await repo.AnyAsync(c => c.Name.ToLower() == name.ToLower() && c.Id != excludingId.Value);
        }
        else
        {
            exists = await repo.AnyAsync(c => c.Name.ToLower() == name.ToLower());
        }

        if (exists)
        {
            throw new ValidationException($"Cinema with the name '{name}' already exists.");
        }
    }

    public override async Task<CinemaResponse> CreateAsync(CreateCinemaDto request)
    {
        await ValidateUniqueCinemaAsync(request.Name);

        var entity = MapToEntity(request);
        await unitOfWork.Repository<Cinema>().AddAsync(entity);
        await unitOfWork.CompleteAsync();

        return MapToResponse(entity);
    }

    public override async Task<CinemaResponse> UpdateAsync(Guid id, UpdateCinemaDto request)
    {
        var entity = await unitOfWork.Repository<Cinema>().GetByIdAsync(id);
        if (entity == null)
            throw new KeyNotFoundException("Cinema not found.");

        await ValidateUniqueCinemaAsync(request.Name, id);

        UpdateEntity(entity, request);
        unitOfWork.Repository<Cinema>().Update(entity);
        await unitOfWork.CompleteAsync();

        return MapToResponse(entity);
    }
}
