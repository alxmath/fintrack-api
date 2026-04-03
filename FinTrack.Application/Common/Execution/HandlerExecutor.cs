using FinTrack.Application.Common.Behaviors;
using FinTrack.Application.Common.Results;
using FluentValidation;

namespace FinTrack.Application.Common.Execution;

public class HandlerExecutor
{
    public async Task<Result<TResponse>> Execute<TRequest, TResponse>(
        TRequest request,
        Func<Task<Result<TResponse>>> handler,
        IValidator<TRequest>? validator,
        CancellationToken cancellationToken)
    {
        var validation = await ValidationHelper
            .ValidateAsync(request, validator, cancellationToken);

        if (validation is not null)
            return Result<TResponse>.Failure(
                validation.Error,
                validation.ErrorCode);

        return await handler();
    }
}