using Project_PRN222_G5.DataAccess.Interfaces.Service;

namespace Project_PRN222_G5.DataAccess.Service;

public sealed class DateTimeService : IDateTimeService
{
    public DateTimeOffset NowUtc => DateTimeOffset.Now;

    public DateTime Now => DateTime.Now;
}