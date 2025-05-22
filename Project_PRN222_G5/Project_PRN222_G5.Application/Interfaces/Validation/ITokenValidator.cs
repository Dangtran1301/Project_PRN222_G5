namespace Project_PRN222_G5.Application.Interfaces.Validation;

public interface ITokenValidator
{
    Task<bool> IsRefreshTokenValidAsync(Guid userId, string refreshToken);
}