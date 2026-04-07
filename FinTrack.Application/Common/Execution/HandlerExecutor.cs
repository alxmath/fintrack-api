using FinTrack.Application.Common.Results;

namespace FinTrack.Application.Common.Execution;

public class HandlerExecutor
{
    private readonly IEnumerable<IExecutionStep> _steps;

    public HandlerExecutor(
        IEnumerable<IExecutionStep> steps) => _steps = steps;

    public async Task<Result<object>> Execute<TRequest>(
        TRequest request,
        Func<Task<Result<object>>> handler,
        CancellationToken cancellationToken)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        Func<Task<Result<object>>> pipeline = handler;

        foreach (var step in _steps.Reverse())
        {
            var next = pipeline;
            pipeline = () => step.Execute(request, cancellationToken, next);
        }

        return await pipeline();
    }
}