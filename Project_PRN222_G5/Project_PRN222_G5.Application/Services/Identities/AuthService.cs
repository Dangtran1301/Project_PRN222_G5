using Microsoft.Extensions.Configuration;
using Project_PRN222_G5.BusinessLogic.DTOs.Users.Requests;
using Project_PRN222_G5.BusinessLogic.DTOs.Users.Responses;
using Project_PRN222_G5.BusinessLogic.Exceptions;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Identities;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Jwt;
using Project_PRN222_G5.BusinessLogic.Interfaces.Validation;
using Project_PRN222_G5.BusinessLogic.Mapper.Users;
using Project_PRN222_G5.Infrastructure.Entities.Users;
using Project_PRN222_G5.Infrastructure.Interfaces.Service;
using Project_PRN222_G5.Infrastructure.Interfaces.UnitOfWork;

namespace Project_PRN222_G5.BusinessLogic.Services.Identities;

public class AuthService(
    IUnitOfWork unitOfWork,
    IValidationService validationService,
    IConfiguration configuration,
    IJwtService jwtService,
    IAuthenticatedUserService authenticatedUserService
    ) : GenericService<User, RegisterUserRequest, UpdateInfoUser, UserResponse>(unitOfWork, validationService), IAuthService
{
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var errors = await validationService.ValidateAsync(request);
        if (errors.Any())
        {
            throw new ValidationException(errors);
        }

        var user = (await unitOfWork.Repository<User>()
                .FindAsync(u => u.Username == request.Username))
            .FirstOrDefault();

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new ValidationException(new Dictionary<string, string[]>
            {
                ["Credentials"] = ["Username or Password is not correct."]
            });
        }

        var accessToken = jwtService.GenerateAccessToken(user);
        var refreshToken = Guid.NewGuid().ToString();
        var userId = user.Id;

        await unitOfWork.Repository<UserToken>().AddAsync(new UserToken
        {
            UserId = userId,
            RefreshToken = refreshToken,
            ExpiredTime = DateTimeOffset.UtcNow.AddDays(7),
            CreatedBy = userId,
            ClientIp = authenticatedUserService.ClientIp
        });

        await unitOfWork.CompleteAsync();

        return new LoginResponse { AccessToken = accessToken, RefreshToken = refreshToken };
    }

    public async Task<UserResponse> RegisterUserAsync(RegisterUserRequest request)
    {
        var errors = await validationService.ValidateAsync(request);
        if (errors.Any())
        {
            throw new ValidationException(errors);
        }

        await validationService.ValidateUniqueUserAsync(request.Username, request.Email);

        var user = MapToEntity(request);
        await unitOfWork.Repository<User>().AddAsync(user);
        await unitOfWork.CompleteAsync();

        return MapToResponse(user);
    }

    public async Task LogoutAsync(Guid userId, string refreshToken)
    {
        var tokens = await unitOfWork.Repository<UserToken>()
            .FindAsync(t => t.UserId == userId && t.RefreshToken == refreshToken);
        var token = tokens.FirstOrDefault();
        if (token != null)
        {
            unitOfWork.Repository<UserToken>().Delete(token);
            await unitOfWork.CompleteAsync();
        }
    }

    protected override UserResponse MapToResponse(User entity) => entity.ToResponse();

    protected override User MapToEntity(RegisterUserRequest request) => request.ToEntity();

    protected override void UpdateEntity(User entity, UpdateInfoUser request) => entity.UpdateEntity(request);
}