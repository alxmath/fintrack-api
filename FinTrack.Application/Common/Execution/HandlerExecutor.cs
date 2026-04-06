using FinTrack.Application.Common.Observability;
using FinTrack.Application.Common.Results;
using System.Diagnostics;

namespace FinTrack.Application.Common.Execution;

public class HandlerExecutor
{
    private readonly IEnumerable<IExecutionStep> _steps;

    public HandlerExecutor(
        IEnumerable<IExecutionStep> steps)
    {
        _steps = steps;
    }

    public async Task<Result<object>> Execute<TRequest>(
        TRequest request,
        Func<Task<Result<object>>> handler,
        CancellationToken cancellationToken)
    {
        var rawName = typeof(TRequest).Name;
        var requestName = rawName.Replace("Query", "").Replace("Command", "");

        using var activity = ActivitySources.ApplicationSource.StartActivity(
            requestName,
            ActivityKind.Internal);

        activity?.SetTag("app.request.name", requestName);
        activity?.SetTag("app.request.type", typeof(TRequest).FullName);

        var stopwatch = Stopwatch.StartNew();

        try
        {
            Func<Task<Result<object>>> pipeline = handler;

            foreach (var step in _steps.Reverse())
            {
                var next = pipeline;
                pipeline = () => step.Execute(request!, cancellationToken, next);
            }

            var result = await pipeline();

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