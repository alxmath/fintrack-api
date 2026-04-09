using FinTrack.Application.Common.Results;
using FluentValidation;
using FluentValidation.Results;
using static FinTrack.Application.Common.Errors.Errors;

namespace FinTrack.Application.Common.Execution.Steps;

public class ValidationStep(IServiceProvider serviceProvider) : IExecutionStep
{
    public int Order => 1;

    public async Task<Result<object>> Execute(
        object request,
        CancellationToken cancellationToken,
        Func<Task<Result<object>>> next)
    {
        var validatorType = typeof(IValidator<>).MakeGenericType(request.GetType());
        var validator = serviceProvider.GetService(validatorType);

        if (validator is null)
            return await next();

        // usamos dynamic apenas para chamar o método genérico
        ValidationResult validationResult =
            await ((dynamic)validator).ValidateAsync((dynamic)request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            return Result<object>.Failure(errors, General.Validation);
        }

        return await next();
    }
}
