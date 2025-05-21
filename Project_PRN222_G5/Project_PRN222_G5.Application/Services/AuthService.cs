using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Project_PRN222_G5.Application.DTOs.Users.Requests;
using Project_PRN222_G5.Application.DTOs.Users.Responses;
using Project_PRN222_G5.Application.Exceptions;
using Project_PRN222_G5.Application.Interfaces.Service;
using Project_PRN222_G5.Application.Interfaces.Validation;
using Project_PRN222_G5.Application.Mapper.Users;
using Project_PRN222_G5.Domain.Entities.Users;
using Project_PRN222_G5.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Project_PRN222_G5.Application.Services;

public class AuthService(IUnitOfWork unitOfWork, IValidationService validationService, IConfiguration configuration)
        : GenericService<User, RegisterUserRequest, UpdateInfoUser, UserResponse>(unitOfWork: unitOfWork), IAuthService
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
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        var token = GenerateJwtToken(user);
        var refreshToken = Guid.NewGuid().ToString();
        await unitOfWork.Repository<UserToken>().AddAsync(new UserToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            RefreshToken = refreshToken,
            ExpiredTime = DateTimeOffset.UtcNow.AddDays(7)
        });
        await unitOfWork.CompleteAsync();

        return new LoginResponse { AccessToken = token, RefreshToken = refreshToken };
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

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.Username),
            new Claim(ClaimTypes.Role, string.Join(",", user.Role))
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    protected override UserResponse MapToResponse(User entity) => entity.ToResponse();

    protected override User MapToEntity(RegisterUserRequest request) => request.ToEntity();

    protected override void UpdateEntity(User entity, UpdateInfoUser request) => entity.UpdateEntity(request);
}