namespace Project_PRN222_G5.DataAccess.Interfaces.Service;

public interface IDateTimeService
{
    DateTimeOffset NowUtc { get; }

    DateTime Now { get; }
}