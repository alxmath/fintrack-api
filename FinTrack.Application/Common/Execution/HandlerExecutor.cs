using FinTrack.Application.Common.Results;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using static FinTrack.Application.Common.Errors.Errors;

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
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation(
            "Handling {Request} {@RequestData}",
            requestName,
            request);

        var stopwatch = Stopwatch.StartNew();

        try
        {
            if (validator is not null)
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Validation failed for {Request}", requestName);

                    return Result<object>.Failure(
                        validationResult.Errors.First().ErrorMessage,
                        General.Validation);
                }
            }

            var result = await handler();

            stopwatch.Stop();

            if (result.IsSuccess)
            {
                _logger.LogInformation(
                    "Handled {Request} {@RequestData} in {Elapsed}ms",
                    requestName,
                    request,
                    stopwatch.ElapsedMilliseconds);
            }
            else
            {
                _logger.LogWarning(
                    "Handled {Request} with failure in {Elapsed}ms: {Error}",
                    requestName,
                    stopwatch.ElapsedMilliseconds,
                    result.Error);
            }

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _logger.LogError(ex,
                "Unhandled exception in {Request} {@RequestData} after {Elapsed}ms",
                requestName,
                request,
                stopwatch.ElapsedMilliseconds);

            throw;
        }
    }
}