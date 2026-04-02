using FinTrack.Application.Common.Results;
using FluentValidation;
using static FinTrack.Application.Common.Errors.Errors;

namespace FinTrack.Application.Common.Behaviors;

public static class ValidationHelper
{
    public static async Task<Result?> ValidateAsync<TRequest>(
        TRequest request,
        IValidator<TRequest>? validator,
        CancellationToken cancellationToken)
    {
        if (validator is null)
            return null;

        var result = await validator.ValidateAsync(request, cancellationToken);

        if (result.IsValid)
            return null;

        var error = result.Errors.First().ErrorMessage;

        return Result.Failure(error, General.Validation);
    }
}
