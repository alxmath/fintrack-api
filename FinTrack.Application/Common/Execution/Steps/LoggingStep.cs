using FinTrack.Application.Common.Results;
using Microsoft.Extensions.Logging;

namespace FinTrack.Application.Common.Execution.Steps;

public class LoggingStep(ILogger<LoggingStep> logger) : IExecutionStep
{
    public int Order => 2;

    public async Task<Result<object>> Execute(
        object request,
        CancellationToken cancellationToken,
        Func<Task<Result<object>>> next)
    {
        var requestName = request.GetType().Name;

        logger.LogInformation(
            "Handling {Request} {@RequestData}",
            requestName,
            request);

        var result = await next();

        if (result.IsSuccess)
        {
            logger.LogInformation(
                "Handled {Request} successfully",
                requestName);
        }
        else
        {
            logger.LogWarning(
                "Handled {Request} with failure: {Errors}",
                requestName,
                result.Errors);
        }

        return result;
    }
}