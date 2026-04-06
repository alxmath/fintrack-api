using FinTrack.Application.Common.Observability;
using FinTrack.Application.Common.Results;
using static FinTrack.Application.Common.Errors.Errors;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace FinTrack.Application.Common.Execution;


public class HandlerExecutor
{
    private readonly ILogger<HandlerExecutor> _logger;

    public HandlerExecutor(ILogger<HandlerExecutor> logger)
    {
        _logger = logger;
    }

    public async Task<Result<object>> Execute<TRequest>(
        TRequest request,
        Func<Task<Result<object>>> handler,
        IValidator<TRequest>? validator,
        CancellationToken cancellationToken)
    {
        var rawName = typeof(TRequest).Name;
        var requestName = rawName.Replace("Query", "").Replace("Command", "");

        using var activity = ActivitySources.ApplicationSource.StartActivity(
            requestName,
            ActivityKind.Internal);

        activity?.SetTag("app.request.name", requestName);
        activity?.SetTag("app.request.type", typeof(TRequest).FullName);

        _logger.LogInformation(
            "Handling {Request} {@RequestData}",
            requestName,
            request);

        var stopwatch = Stopwatch.StartNew();

        try
        {
            // Validation
            if (validator is not null)
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    activity?.SetTag("validation.failed", true);
                    activity?.SetStatus(ActivityStatusCode.Error);

                    _logger.LogWarning("Validation failed for {Request}", requestName);

                    var errors = validationResult.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        );

                    return Result<object>.Failure(errors, General.Validation);
                }
            }

            // Execute handler
            var result = await handler();

            stopwatch.Stop();

            activity?.SetTag("execution.time.ms", stopwatch.ElapsedMilliseconds);
            activity?.SetTag("result.success", result.IsSuccess);

            if (result.IsSuccess)
            {
                activity?.SetStatus(ActivityStatusCode.Ok);

                _logger.LogInformation(
                    "Handled {Request} in {Elapsed}ms",
                    requestName,
                    stopwatch.ElapsedMilliseconds);
            }
            else
            {
                activity?.SetStatus(ActivityStatusCode.Error);

                _logger.LogWarning(
                    "Handled {Request} with failure in {Elapsed}ms: {Error}",
                    requestName,
                    stopwatch.ElapsedMilliseconds,
                    result.Errors);
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

            _logger.LogError(ex,
                "Unhandled exception in {Request} after {Elapsed}ms",
                requestName,
                stopwatch.ElapsedMilliseconds);

            throw;
        }
    }
}