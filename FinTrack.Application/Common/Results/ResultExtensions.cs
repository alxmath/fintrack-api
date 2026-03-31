using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Application.Common.Results;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        return result.IsSuccess
            ? new OkObjectResult(result.Value)
            : new BadRequestObjectResult(new { error = result.Error });
    }
}
