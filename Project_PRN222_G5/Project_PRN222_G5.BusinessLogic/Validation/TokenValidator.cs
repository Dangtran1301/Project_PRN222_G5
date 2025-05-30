using Project_PRN222_G5.BusinessLogic.Interfaces.Validation;
using Project_PRN222_G5.DataAccess.Entities.Users;
using Project_PRN222_G5.DataAccess.Interfaces.Service;
using Project_PRN222_G5.DataAccess.Interfaces.UnitOfWork;

namespace Project_PRN222_G5.BusinessLogic.Validation;

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