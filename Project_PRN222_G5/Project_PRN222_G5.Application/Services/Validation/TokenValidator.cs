using Project_PRN222_G5.BusinessLogic.Interfaces.Validation;
using Project_PRN222_G5.Infrastructure.Entities.Users;
using Project_PRN222_G5.Infrastructure.Interfaces.Service;
using Project_PRN222_G5.Infrastructure.Interfaces.UnitOfWork;

namespace Project_PRN222_G5.BusinessLogic.Services.Validation;

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