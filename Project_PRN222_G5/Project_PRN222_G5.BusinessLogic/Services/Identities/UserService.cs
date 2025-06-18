using Project_PRN222_G5.BusinessLogic.Interfaces.Service;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Identities;
using Project_PRN222_G5.BusinessLogic.Interfaces.Validation;
using Project_PRN222_G5.BusinessLogic.Mapper.Users;
using Project_PRN222_G5.DataAccess.DTOs.Users.Requests;
using Project_PRN222_G5.DataAccess.DTOs.Users.Responses;
using Project_PRN222_G5.DataAccess.Entities.Users;
using Project_PRN222_G5.DataAccess.Exceptions;
using Project_PRN222_G5.DataAccess.Interfaces.UnitOfWork;

namespace Project_PRN222_G5.BusinessLogic.Services.Identities;

public class UserService(
    IUnitOfWork unitOfWork,
    IValidationService validationService,
    IMediaService mediaService
    ) : GenericService<User, RegisterUserRequest, UpdateInfoUser, UserResponse>(unitOfWork, validationService), IUserService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IValidationService _validationService = validationService;

    public override UserResponse MapToResponse(User entity) => entity.ToResponse();

    public override void UpdateEntity(User entity, UpdateInfoUser request) => entity.UpdateEntity(request, mediaService);

    public override User MapToEntity(RegisterUserRequest request) => request.ToEntity();

    public async Task<UserResponse> GetUserInfoById(Guid id)
    {
        return MapToResponse(await _unitOfWork.Repository<User>().GetByIdAsync(id));
    }

    public async Task<bool> ResetPassword(Guid id, ResetPasswordRequest request)
    {
        if (!_validationService.TryValidate(request, out var errors))
            throw new ValidationException(errors);

        var user = await _unitOfWork.Repository<User>().GetByIdAsync(id);

        if (!BCrypt.Net.BCrypt.Verify(request.OldPassword, user.PasswordHash))
        {
            throw new ValidationException("Old password is not correct.");
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.ConfirmPassword);
        _unitOfWork.Repository<User>().Update(user);

        _unitOfWork.Repository<UserToken>().RemoveRange(
            await _unitOfWork.Repository<UserToken>().FindAsync(x => x.UserId == user.Id)
            );
        await _unitOfWork.CompleteAsync();
        return true;
    }

    public override async Task<UserResponse> UpdateAsync(Guid id, UpdateInfoUser request)
    {
        if (!_validationService.TryValidate(request, out var errors))
            throw new ValidationException(errors);

        var entity = await _unitOfWork.Repository<User>().GetByIdAsync(id);
        if (entity == null)
        {
            throw new ValidationException("User not found.");
        }

        entity.UpdateEntity(request, mediaService);

        if (request.Avatar is not null)
        {
            if (!string.IsNullOrEmpty(entity.Avatar))
            {
                await mediaService.DeleteImageAsync(entity.Avatar);
            }

            entity.Avatar = await mediaService.UploadImageAsync(request.Avatar, nameof(request.Avatar));
        }
        _unitOfWork.Repository<User>().Update(entity);

        await _unitOfWork.CompleteAsync();

        return MapToResponse(entity);
    }
}