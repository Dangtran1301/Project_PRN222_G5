using Project_PRN222_G5.Application.Interfaces.Service;
using Project_PRN222_G5.Application.Interfaces.UnitOfWork;
using Project_PRN222_G5.Application.Interfaces.Validation;
using Project_PRN222_G5.Domain.Entities.Users;

namespace Project_PRN222_G5.Application.Services.Validation;

public class TokenValidator(IUnitOfWork unitOfWork, IDateTimeService timeService) : ITokenValidator
{
    public async Task<bool> IsRefreshTokenValidAsync(Guid userId, string refreshToken)
    {
        return (await unitOfWork.Repository<UserToken>()
             .FindAsync(x => x.UserId == userId
                             && x.RefreshToken == refreshToken
                             && x.ExpiredTime > timeService.NowUtc)).Any();
    }
}