using FluentValidation;
using Project_PRN222_G5.Application.DTOs.Requests;
using Project_PRN222_G5.Application.DTOs.Users.Responses;
using Project_PRN222_G5.Application.Interfaces;
using Project_PRN222_G5.Application.Mapper.Users;
using Project_PRN222_G5.Domain.Entities.Users;
using Project_PRN222_G5.Domain.Interfaces;

namespace Project_PRN222_G5.Application.Services;

public class UserService(IUnitOfWork unitOfWork, IValidator<RegisterUserRequest> validator)
    : GenericService<User, RegisterUserRequest, UpdateInfoUser, UserResponse>(unitOfWork), IUserService
{
    public async Task<UserResponse> RegisterUserAsync(RegisterUserRequest request)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new ValidationException((IEnumerable<FluentValidation.Results.ValidationFailure>)
                validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }
        var userRepository = unitOfWork.Repository<User>();
        var existingUser = (await userRepository.FindAsync(u => u.Username == request.Username)).FirstOrDefault()
                           ?? (await userRepository.FindAsync(u => u.Email == request.Email)).FirstOrDefault();
        if (existingUser != null)
        {
            throw new InvalidOperationException("Username or email already exists.");
        }

        var user = MapToEntity(request);

        await userRepository.AddAsync(user);
        await unitOfWork.CompleteAsync();

        return MapToResponse(user);
    }

    public async Task<IEnumerable<UserResponse>> GetPagedAsync(int page, int pageSize)
    {
        return await base.GetPagedAsync(page, pageSize);
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
        var user = (await unitOfWork.Repository<User>().FindAsync(u => u.Username == username)).FirstOrDefault();
        return user ?? throw new KeyNotFoundException($"User with username {username} not found.");
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        var user = (await unitOfWork.Repository<User>().FindAsync(u => u.Email == email)).FirstOrDefault();
        return user ?? throw new KeyNotFoundException($"User with email {email} not found.");
    }

    protected override UserResponse MapToResponse(User entity) => entity.ToResponse();

    protected override User MapToEntity(RegisterUserRequest request) => request.ToEntity();

    protected override void UpdateEntity(User entity, UpdateInfoUser request) => entity.UpdateEntity(request);
}