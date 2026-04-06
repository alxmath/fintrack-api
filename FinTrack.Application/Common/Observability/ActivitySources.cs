using System.Diagnostics;

namespace FinTrack.Application.Common.Observability;

public static class ActivitySources
{
    public const string Application = "FinTrack.Application";
    public const string Infrastructure = "FinTrack.Infrastructure";

    public static readonly ActivitySource ApplicationSource = new(Application);
    public static readonly ActivitySource InfrastructureSource = new(Infrastructure);
}
