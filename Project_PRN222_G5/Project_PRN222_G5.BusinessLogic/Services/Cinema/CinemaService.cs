using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Cinema;
using Project_PRN222_G5.BusinessLogic.Interfaces.Validation;
using Project_PRN222_G5.BusinessLogic.Mapper.Cinema;
using Project_PRN222_G5.DataAccess.DTOs.Cinema.Request;
using Project_PRN222_G5.DataAccess.DTOs.Cinema.Response;
using Project_PRN222_G5.DataAccess.Exceptions;
using Project_PRN222_G5.DataAccess.Interfaces.UnitOfWork;
using System.Linq.Expressions;

namespace Project_PRN222_G5.BusinessLogic.Services.Cinema;

public class CinemaService(
    IUnitOfWork unitOfWork,
    IValidationService validationService
) : GenericService<DataAccess.Entities.Cinemas.Cinema, CreateCinemaDto, UpdateCinemaDto, CinemaResponse>(unitOfWork, validationService), ICinemaService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IValidationService _validationService = validationService;

    public override CinemaResponse MapToResponse(DataAccess.Entities.Cinemas.Cinema entity) => entity.ToCinemaResponse();

    public override DataAccess.Entities.Cinemas.Cinema MapToEntity(CreateCinemaDto request) => request.ToEntity();

    public override void UpdateEntity(DataAccess.Entities.Cinemas.Cinema entity, UpdateCinemaDto request) => entity.UpdateEntity(request);

    public override async Task<CinemaResponse> CreateAsync(CreateCinemaDto request, CancellationToken cancellationToken = default)
    {
        await _validationService.ValidateUniqueCinemaAsync(request.Name);

        var entity = request.ToEntity();
        await _unitOfWork.Repository<DataAccess.Entities.Cinemas.Cinema>().AddAsync(entity, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return entity.ToCinemaResponse();
    }

    public override async Task<CinemaResponse> UpdateAsync(Guid id, UpdateCinemaDto request, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Repository<DataAccess.Entities.Cinemas.Cinema>().GetByIdAsync(id, cancellationToken, true)
                     ?? throw new ValidationException("Cinema not found.");
        await _validationService.ValidateUniqueCinemaAsync(request.Name, id);

        entity.Name = request.Name;
        entity.Address = request.Address;
        _unitOfWork.Repository<DataAccess.Entities.Cinemas.Cinema>().Update(entity);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return entity.ToCinemaResponse();
    }

    public override async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _validationService.ValidateCinemaCanBeDeletedAsync(id);

        var cinema = await _unitOfWork.Repository<DataAccess.Entities.Cinemas.Cinema>().GetByIdAsync(id, cancellationToken, true);
        _unitOfWork.Repository<DataAccess.Entities.Cinemas.Cinema>().Delete(cinema);
        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    protected override Expression<Func<DataAccess.Entities.Cinemas.Cinema, string>>[] DefineSearchFields() =>
    [
        x=>x.Address,
        x=>x.Name
    ];
}