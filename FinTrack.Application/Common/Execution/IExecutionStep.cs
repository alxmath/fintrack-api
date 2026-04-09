using FinTrack.Application.Common.Results;

namespace FinTrack.Application.Common.Execution;

public interface IExecutionStep
{
    int Order { get; }
    
    Task<Result<object>> Execute(
        object request,
        CancellationToken cancellationToken,
        Func<Task<Result<object>>> next);
}
