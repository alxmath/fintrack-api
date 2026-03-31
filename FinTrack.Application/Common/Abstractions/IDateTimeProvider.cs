namespace FinTrack.Application.Common.Abstractions;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
