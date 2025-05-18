using FluentValidation;
using Project_PRN222_G5.Application.DTOs.Requests;
using Project_PRN222_G5.Application.DTOs.Responses;
using Project_PRN222_G5.Application.Interfaces;
using Project_PRN222_G5.Domain.Entities.Users;
using Project_PRN222_G5.Domain.Entities.Users.Enum;
using Project_PRN222_G5.Domain.Interfaces;

namespace Project_PRN222_G5.Application.Services;

public class UserService(IUnitOfWork unitOfWork, IValidator<RegisterUserRequest> validator)
    : GenericService<User, RegisterUserRequest, UserResponse>(unitOfWork), IUserService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<UserResponse> RegisterUserAsync(RegisterUserRequest request)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new ValidationException((IEnumerable<FluentValidation.Results.ValidationFailure>)
                validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }

        var userRepository = _unitOfWork.Repository<User>();
        var existingUser = (await userRepository.FindAsync(u => u.Username == request.Username)).FirstOrDefault()
                           ?? (await userRepository.FindAsync(u => u.Email == request.Email)).FirstOrDefault();
        if (existingUser != null)
        {
            throw new InvalidOperationException("Username or email already exists.");
        }

        var user = MapToEntity(request);

        await userRepository.AddAsync(user);
        await _unitOfWork.CompleteAsync();

        return MapToResponse(user);
    }

    public async Task<IEnumerable<UserResponse>> GetPagedAsync(int page, int pageSize)
    {
        return await base.GetPagedAsync(page, pageSize);
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
        var user = (await _unitOfWork.Repository<User>().FindAsync(u => u.Username == username)).FirstOrDefault();
        return user ?? throw new KeyNotFoundException($"User with username {username} not found.");
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        var user = (await _unitOfWork.Repository<User>().FindAsync(u => u.Email == email)).FirstOrDefault();
        return user ?? throw new KeyNotFoundException($"User with email {email} not found.");
    }

    protected override UserResponse MapToResponse(User entity) =>
        new()
        {
            Id = entity.Id,
            FullName = entity.FullName,
            Username = entity.Username,
            Email = entity.Email,
            Role = entity.Role.ToString(),
        };

    protected override User MapToEntity(RegisterUserRequest request) =>
        new()
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            Username = request.Username,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = request.Role.Any()
                ? Enum.Parse<Role>(request.Role, true)
                : Role.Customer,
            CreatedAt = DateTimeOffset.UtcNow,
            CreatedBy = "System"
        };

    protected override void UpdateEntity(User entity, RegisterUserRequest request)
    {
        entity.FullName = request.FullName;
        entity.Username = request.Username;
        entity.Email = request.Email;
        entity.Role = request.Role.Any()
            ? Enum.Parse<Role>(request.Role, true)
            : Role.Customer;
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        entity.UpdatedBy = string.Empty;
    }
}