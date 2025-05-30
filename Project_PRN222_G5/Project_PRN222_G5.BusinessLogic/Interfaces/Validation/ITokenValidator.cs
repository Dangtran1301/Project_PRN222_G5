namespace Project_PRN222_G5.BusinessLogic.Interfaces.Validation;

public interface ITokenValidator
{
    Task<bool> IsRefreshTokenValidAsync(Guid userId, string refreshToken);
}