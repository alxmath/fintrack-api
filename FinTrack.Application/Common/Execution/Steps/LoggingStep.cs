using FinTrack.Application.Common.Results;
using Microsoft.Extensions.Logging;

namespace FinTrack.Application.Common.Execution.Steps;

public class LoggingStep : IExecutionStep
{
    private readonly ILogger<LoggingStep> _logger;

    public LoggingStep(ILogger<LoggingStep> logger)
    {
        _logger = logger;
    }

    public async Task<Result<object>> Execute(
        object request,
        CancellationToken cancellationToken,
        Func<Task<Result<object>>> next)
    {
        var requestName = request.GetType().Name;

        _logger.LogInformation(
            "Handling {Request} {@RequestData}",
            requestName,
            request);

        var result = await next();

        if (result.IsSuccess)
        {
            _logger.LogInformation(
                "Handled {Request} successfully",
                requestName);
        }
        else
        {
            _logger.LogWarning(
                "Handled {Request} with failure: {Errors}",
                requestName,
                result.Errors);
        }

        return result;
    }
}