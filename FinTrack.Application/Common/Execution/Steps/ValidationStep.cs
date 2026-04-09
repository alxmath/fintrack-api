using FinTrack.Application.Common.Results;
using FluentValidation;
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

        if (validator is not null)
        {
            var method = validatorType
                .GetMethod("ValidateAsync", [request.GetType(), typeof(CancellationToken)]);

            var task = (Task)method!.Invoke(validator, [request, cancellationToken])!;

            await task;

            var resultProperty = task.GetType().GetProperty("Result");
            var validationResult = resultProperty!.GetValue(task);

            var isValid = (bool)validationResult!.GetType().GetProperty("IsValid")!.GetValue(validationResult)!;

            if (!isValid)
            {
                var errorsProperty = validationResult.GetType().GetProperty("Errors");
                var errorsList = (IEnumerable<object>)errorsProperty!.GetValue(validationResult)!;

                var errors = errorsList
                    .Select(e => new
                    {
                        PropertyName = (string)e.GetType().GetProperty("PropertyName")!.GetValue(e)!,
                        ErrorMessage = (string)e.GetType().GetProperty("ErrorMessage")!.GetValue(e)!
                    })
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                return Result<object>.Failure(errors, General.Validation);
            }
        }

        return await next();
    }
}
