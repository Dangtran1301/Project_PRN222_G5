using Microsoft.Extensions.Configuration;
using Project_PRN222_G5.BusinessLogic.DTOs.Users.Requests;
using Project_PRN222_G5.BusinessLogic.DTOs.Users.Responses;
using Project_PRN222_G5.BusinessLogic.Exceptions;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Identities;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Jwt;
using Project_PRN222_G5.BusinessLogic.Interfaces.Validation;
using Project_PRN222_G5.BusinessLogic.Mapper.Users;
using Project_PRN222_G5.DataAccess.Entities.Identities.Users;
using Project_PRN222_G5.DataAccess.Interfaces.Service;
using Project_PRN222_G5.DataAccess.Interfaces.UnitOfWork;

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

    public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var token = (await unitOfWork.Repository<UserToken>()
                .FindAsync(t => t.UserId == request.UserId && t.RefreshToken == request.RefreshToken))
            .FirstOrDefault();

        if (token == null || token.ExpiredTime < DateTimeOffset.UtcNow)
        {
            throw new ValidationException(new Dictionary<string, string[]>
            {
                ["RefreshToken"] = ["Invalid or expired refresh token."]
            });
        }

        var user = await unitOfWork.Repository<User>().GetByIdAsync(request.UserId);
        if (user == null)
        {
            throw new ValidationException(new Dictionary<string, string[]>
            {
                ["User"] = ["User not found."]
            });
        }

        var newAccessToken = jwtService.GenerateAccessToken(user);
        var newRefreshToken = Guid.NewGuid().ToString();

        token.RefreshToken = newRefreshToken;
        token.ExpiredTime = DateTimeOffset.UtcNow.AddDays(7);
        token.ClientIp = authenticatedUserService.ClientIp;

        unitOfWork.Repository<UserToken>().Update(token);
        await unitOfWork.CompleteAsync();

        return new LoginResponse { AccessToken = newAccessToken, RefreshToken = newRefreshToken };
    }

    protected override UserResponse MapToResponse(User entity) => entity.ToResponse();

    protected override User MapToEntity(RegisterUserRequest request) => request.ToEntity();

    protected override void UpdateEntity(User entity, UpdateInfoUser request) => entity.UpdateEntity(request);
}