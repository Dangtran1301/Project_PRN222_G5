using Project_PRN222_G5.Application.Interfaces.Service;

namespace Project_PRN222_G5.Infrastructure.Service;

public class DateTimeService : IDateTimeService
{
    public DateTime NowUtc => DateTime.UtcNow;
}