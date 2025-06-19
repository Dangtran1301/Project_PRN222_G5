using Project_PRN222_G5.BusinessLogic.Interfaces.Service;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service.Identities;
using Project_PRN222_G5.BusinessLogic.Interfaces.Validation;
using Project_PRN222_G5.BusinessLogic.Mapper.Users;
using Project_PRN222_G5.DataAccess.DTOs.Users.Requests;
using Project_PRN222_G5.DataAccess.DTOs.Users.Responses;
using Project_PRN222_G5.DataAccess.Entities.Users;
using Project_PRN222_G5.DataAccess.Exceptions;
using Project_PRN222_G5.DataAccess.Interfaces.Service;
using Project_PRN222_G5.DataAccess.Interfaces.UnitOfWork;
using System.Linq.Expressions;

namespace Project_PRN222_G5.BusinessLogic.Services.Identities;

public class AuthService(
    IUnitOfWork unitOfWork,
    IValidationService validationService,
    IJwtService jwtService,
    IDateTimeService dateTimeService,
    IAuthenticatedUserService authenticatedUserService,
    IMediaService mediaService,
    ITokenValidator tokenValidator
    ) : GenericService<User, RegisterUserRequest, UpdateInfoUser, UserResponse>(unitOfWork, validationService), IAuthService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IValidationService _validationService = validationService;

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        if (!_validationService.TryValidate(request, out var errors))
            throw new ValidationException(errors);

        var user = (await _unitOfWork.Repository<User>()
                .FindAsync(u => (u.Username == request.Username) || u.Email == request.Username, cancellationToken))
            .FirstOrDefault();

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new ValidationException(new Dictionary<string, string[]>
            {
                ["Credentials"] = ["Username/Email or Password is not correct."]
            });
        }

        var accessToken = jwtService.GenerateAccessToken(user);
        var refreshToken = Guid.NewGuid().ToString();
        var userId = user.Id;

        await _unitOfWork.Repository<UserToken>().AddAsync(new UserToken
        {
            UserId = userId,
            RefreshToken = refreshToken,
            ExpiredTime = dateTimeService.NowUtc.AddDays(7),
            CreatedBy = userId,
            ClientIp = authenticatedUserService.ClientIp
        },
            cancellationToken);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return new LoginResponse { AccessToken = accessToken, RefreshToken = refreshToken };
    }

    public override async Task<UserResponse> CreateAsync(RegisterUserRequest request, CancellationToken cancellationToken = default)
    {
        if (!_validationService.TryValidate(request, out var errors))
            throw new ValidationException(errors);

        await _validationService.ValidateUniqueUserAsync(request.Username, request.Email);

        var user = MapToEntity(request);
        await _unitOfWork.Repository<User>().AddAsync(user, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return MapToResponse(user);
    }

    public async Task LogoutAsync(Guid userId, string refreshToken, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(refreshToken)) return;

        var tokens = await _unitOfWork.Repository<UserToken>()
            .FindAsync(t => t.UserId == userId && t.RefreshToken == refreshToken, cancellationToken);

        _unitOfWork.Repository<UserToken>().RemoveRange(tokens);

        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        if (!await tokenValidator.IsRefreshTokenValidAsync(request.UserId, request.RefreshToken))
        {
            throw new ValidationException(new Dictionary<string, string[]>
            {
                ["RefreshToken"] = ["Invalid or expired refresh token."]
            });
        }

        var user = await _unitOfWork.Repository<User>().GetByIdAsync(request.UserId)
                   ?? throw new ValidationException(new Dictionary<string, string[]>
                   {
                       ["User"] = ["User not found."]
                   });
        var newAccessToken = jwtService.GenerateAccessToken(user);
        var newRefreshToken = Guid.NewGuid().ToString();

        var token = (await _unitOfWork.Repository<UserToken>()
                .FindAsync(t => t.UserId == request.UserId && t.RefreshToken == request.RefreshToken))
            .First();

        token.RefreshToken = newRefreshToken;
        token.ExpiredTime = dateTimeService.NowUtc.AddDays(7);
        token.ClientIp = authenticatedUserService.ClientIp;

        _unitOfWork.Repository<UserToken>().Update(token);
        await _unitOfWork.CompleteAsync();

        return new LoginResponse { AccessToken = newAccessToken, RefreshToken = newRefreshToken };
    }

    public async Task<UserResponse> GetUserByUsernameAsync(string username)
    {
        return MapToResponse(
            (await _unitOfWork.Repository<User>()
                .FindAsync(x => x.Username == username))
                .FirstOrDefault(new User()));
    }

    public override async Task<UserResponse> UpdateAsync(Guid id, UpdateInfoUser request, CancellationToken cancellationToken = default)
    {
        if (!_validationService.TryValidate(request, out var errors))
            throw new ValidationException(errors);

        var entity = await _unitOfWork.Repository<User>().GetByIdAsync(id, cancellationToken, true)
                     ?? throw new ValidationException("User not found.");
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

        await _unitOfWork.CompleteAsync(cancellationToken);

        return MapToResponse(entity);
    }

    public override UserResponse MapToResponse(User entity) => entity.ToResponse();

    public override User MapToEntity(RegisterUserRequest request) => request.ToEntity();

    public override void UpdateEntity(User entity, UpdateInfoUser request) => entity.UpdateEntity(request, mediaService);

    protected override Expression<Func<User, string>>[] DefineSearchFields() =>
    [
        u => u.Username,
        u => u.FullName,
        u => u.Email,
        u => u.PhoneNumber
    ];
}