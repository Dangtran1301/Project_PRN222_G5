using Project_PRN222_G5.DataAccess.Interfaces.Service;

namespace Project_PRN222_G5.DataAccess.Service;

public class DateTimeService : IDateTimeService
{
    public DateTime NowUtc => DateTime.UtcNow;
}