using System.Diagnostics;

namespace FinTrack.Application.Common.Observability;

public static class ActivitySources
{
    public const string Name = "FinTrack.Application";

    public static readonly ActivitySource Source = new(Name);
}
