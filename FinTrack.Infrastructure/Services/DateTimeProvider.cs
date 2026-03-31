using FinTrack.Application.Common.Abstractions;

namespace FinTrack.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
