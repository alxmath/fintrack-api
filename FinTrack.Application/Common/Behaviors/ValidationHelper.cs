using FinTrack.Application.Common.Results;
using FluentValidation;
using static FinTrack.Application.Common.Errors.Errors;

namespace FinTrack.Application.Common.Behaviors;

public static class ValidationHelper
{
    public static async Task<Result<TResponse>> ValidateAsync<TRequest, TResponse>(
        TRequest request,
        IValidator<TRequest>? validator,
        CancellationToken cancellationToken)
    {
        if (validator is null)
            return Result<TResponse>.Success(default!);

        var result = await validator.ValidateAsync(request, cancellationToken);

        if (result.IsValid)
            return Result<TResponse>.Success(default!);

        var error = result.Errors.First().ErrorMessage;

        return Result<TResponse>.Failure(error, General.Validation);
    }
}
