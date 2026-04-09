using FinTrack.Application.Common.Results;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace FinTrack.Application.Common.Execution.Steps;

public class ExceptionStep(ILogger<ExceptionStep> logger) : IExecutionStep
{
    public int Order => 5;

    public async Task<Result<object>> Execute(
        object request,
        CancellationToken cancellationToken,
        Func<Task<Result<object>>> next)
    {
        var requestName = request.GetType().Name;

        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            // Enriquecimento do trace atual (se existir)
            var activity = Activity.Current;

            activity?.SetStatus(ActivityStatusCode.Error);
            activity?.SetTag("exception.type", ex.GetType().FullName);
            activity?.SetTag("exception.message", ex.Message);
            activity?.SetTag("exception.stacktrace", ex.StackTrace);

            // Logging estruturado
            logger.LogError(
                ex,
                "Unhandled exception in {Request}",
                requestName);

            throw;
        }
    }
}