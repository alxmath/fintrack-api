using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Api.Common.Errors;

public static class ProblemDetailsMapper
{
    public static ProblemDetails Map(string error, string errorCode)
    {
        return errorCode switch
        {
            "VALIDATION_ERROR" => new ProblemDetails
            {
                Title = "Validation error",
                Detail = error,
                Status = StatusCodes.Status400BadRequest,
                Type = "https://httpstatuses.com/400"
            },

            "NOT_FOUND" => new ProblemDetails
            {
                Title = "Resource not found",
                Detail = error,
                Status = StatusCodes.Status404NotFound,
                Type = "https://httpstatuses.com/404"
            },

            "CONFLICT" => new ProblemDetails
            {
                Title = "Conflict",
                Detail = error,
                Status = StatusCodes.Status409Conflict,
                Type = "https://httpstatuses.com/409"
            },

            _ => new ProblemDetails
            {
                Title = "Unexpected error",
                Detail = error,
                Status = StatusCodes.Status500InternalServerError,
                Type = "https://httpstatuses.com/500"
            }
        };
    }
}
