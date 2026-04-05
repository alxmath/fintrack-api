using FinTrack.Api.Common.Errors;
using FinTrack.Application.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Api.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult(
       this Result<object> result,
       HttpContext httpContext,
       Func<object, IActionResult>? onSuccess = null)
    {
        if (result.Value is not null && result.IsSuccess)
        {
            return onSuccess != null
                ? onSuccess(result.Value)
                : new OkObjectResult(result.Value);
        }

        var problem = ProblemDetailsMapper.Map(result.Errors, result.ErrorCode);

        problem.Extensions["traceId"] = httpContext.TraceIdentifier;

        return new ObjectResult(problem)
        {
            StatusCode = problem.Status
        };
    }
}
