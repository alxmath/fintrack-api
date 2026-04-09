using System.Diagnostics.Metrics;

namespace FinTrack.Application.Common.Observability;

public static class Metrics
{
    public const string MeterName = "FinTrack.Application";

    private static readonly Meter Meter = new(MeterName);

    public static readonly Counter<long> Requests =
        Meter.CreateCounter<long>("fintrack_requests_total");

    public static readonly Counter<long> Errors =
        Meter.CreateCounter<long>("fintrack_errors_total");

    public static readonly Histogram<double> Duration =
        Meter.CreateHistogram<double>("fintrack_request_duration_ms");
}