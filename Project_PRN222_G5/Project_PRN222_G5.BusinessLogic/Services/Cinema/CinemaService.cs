using Project_PRN222_G5.BusinessLogic.DTOs.Cinema.Request;
using Project_PRN222_G5.BusinessLogic.DTOs.Cinema.Response;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Cinema;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Identities;
using Project_PRN222_G5.BusinessLogic.Interfaces.Validation;
using Project_PRN222_G5.BusinessLogic.Mapper.Cinema;
using Project_PRN222_G5.DataAccess.Entities.Cinemas;
using Project_PRN222_G5.DataAccess.Interfaces.UnitOfWork;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;

namespace Project_PRN222_G5.BusinessLogic.Services.Identities;

public class CinemaService(
    IUnitOfWork unitOfWork,
    IValidationService validationService
) : GenericService<Cinema, CreateCinemaDto, UpdateCinemaDto, CinemaResponse>(unitOfWork, validationService), ICinemaService
{
    public override CinemaResponse MapToResponse(Cinema entity) => entity.ToCinemaResponse();

    public override Cinema MapToEntity(CreateCinemaDto request) => request.ToEntity();

    public override void UpdateEntity(Cinema entity, UpdateCinemaDto request) => entity.UpdateEntity(request);

    public async Task<CinemaResponse> CreateAsync(CreateCinemaDto request)
    {
        await validationService.ValidateUniqueCinemaAsync(request.Name);

        var entity = request.ToEntity();
        await unitOfWork.Repository<Cinema>().AddAsync(entity);
        await unitOfWork.CompleteAsync();

        return entity.ToCinemaResponse();
    }

    public async Task<CinemaResponse> UpdateAsync(Guid id, UpdateCinemaDto request)
    {
        var entity = await unitOfWork.Repository<Cinema>().GetByIdAsync(id);
        if (entity == null)
            throw new KeyNotFoundException("Cinema not found.");

        await validationService.ValidateUniqueCinemaAsync(request.Name, id);

        entity.Name = request.Name;
        entity.Address = request.Address;
        unitOfWork.Repository<Cinema>().Update(entity);
        await unitOfWork.CompleteAsync();

        return entity.ToCinemaResponse();
    }

    protected override Expression<Func<DataAccess.Entities.Cinemas.Cinema, string>>[] GetSearchFields() =>
        [
            x=>x.Address,
            x=>x.Name
        ];
}
