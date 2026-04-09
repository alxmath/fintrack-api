using System.Diagnostics;
using FinTrack.Application.Common.Observability;
using FinTrack.Application.Common.Results;

namespace FinTrack.Application.Common.Execution.Steps;

public class MetricsStep : IExecutionStep
{
    public int Order => 4;

    public async Task<Result<object>> Execute(
        object request,
        CancellationToken cancellationToken,
        Func<Task<Result<object>>> next)
    {
        var requestName = request.GetType().Name;

        var stopwatch = Stopwatch.StartNew();

        try
        {
            var result = await next();

            stopwatch.Stop();

            Metrics.Requests.Add(1,
                new KeyValuePair<string, object?>("request", requestName));

            Metrics.Duration.Record(stopwatch.Elapsed.TotalMilliseconds,
                new KeyValuePair<string, object?>("request", requestName),
                new KeyValuePair<string, object?>("success", result.IsSuccess));

            if (!result.IsSuccess)
            {
                Metrics.Errors.Add(1,
                    new KeyValuePair<string, object?>("request", requestName));
            }

            return result;
        }
        catch
        {
            stopwatch.Stop();

            Metrics.Errors.Add(1,
                new KeyValuePair<string, object?>("request", requestName));

            throw;
        }
    }
}