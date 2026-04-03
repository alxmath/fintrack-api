using FinTrack.Api.Common.Errors;
using FinTrack.Application.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Api.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return new OkObjectResult(result);
        }

        var problem = ProblemDetailsMapper.Map(result.Error, result.ErrorCode);

        return new ObjectResult(problem)
        {
            StatusCode = problem.Status
        };
    }
}
