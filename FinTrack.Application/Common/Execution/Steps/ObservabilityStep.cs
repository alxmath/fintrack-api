namespace FinTrack.Application.Common.Execution.Steps;

using System.Diagnostics;
using FinTrack.Application.Common.Observability;
using FinTrack.Application.Common.Results;

public class ObservabilityStep : IExecutionStep
{
    public async Task<Result<object>> Execute(
        object request,
        CancellationToken cancellationToken,
        Func<Task<Result<object>>> next)
    {
        var rawName = request.GetType().Name;
        var requestName = rawName.Replace("Query", "").Replace("Command", "");

        using var activity = ActivitySources.ApplicationSource.StartActivity(
            requestName,
            ActivityKind.Internal);

        activity?.SetTag("app.request.name", requestName);
        activity?.SetTag("app.request.type", request.GetType().FullName);

        var stopwatch = Stopwatch.StartNew();

        try
        {
            var result = await next();

            stopwatch.Stop();

            activity?.SetTag("execution.time.ms", stopwatch.ElapsedMilliseconds);
            activity?.SetTag("result.success", result.IsSuccess);

            if (result.IsSuccess)
            {
                activity?.SetStatus(ActivityStatusCode.Ok);
            }
            else
            {
                activity?.SetStatus(ActivityStatusCode.Error);
            }

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            activity?.SetStatus(ActivityStatusCode.Error);

            activity?.SetTag("exception.type", ex.GetType().FullName);
            activity?.SetTag("exception.message", ex.Message);
            activity?.SetTag("exception.stacktrace", ex.StackTrace);

            throw;
        }
    }
}
