using FinTrack.Application.Common.Errors;
using FinTrack.Application.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Api.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(
        this Result<T> result,
        Func<T, IActionResult>? onSuccess = null)
    {
        if (result.IsFailure)
        {
            return result.ErrorCode switch
            {
                Errors.General.NotFound => new NotFoundObjectResult(result),
                Errors.General.Validation => new BadRequestObjectResult(result),
                _ => new BadRequestObjectResult(result)
            };
        }

        if (onSuccess is not null)
        {
            return onSuccess(result.Value!);
        }

        return new OkObjectResult(result);
    }

    public static IActionResult ToActionResult(
        this Result result)
    {
        if (result.IsFailure)
        {
            return result.ErrorCode switch
            {
                Errors.General.NotFound => new NotFoundObjectResult(result),
                Errors.General.Validation => new BadRequestObjectResult(result),
                _ => new BadRequestObjectResult(result)
            };
        }

        return new OkObjectResult(result);
    }
}
