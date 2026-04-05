using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Api.Common.Errors;

public static class ProblemDetailsMapper
{
    public static ProblemDetails Map(Dictionary<string, string[]> errors, string errorCode)
    {
        return errorCode switch
        {
            "VALIDATION_ERROR" => new ValidationProblemDetails(errors)
            {
                Title = "Validation error",
                Status = StatusCodes.Status400BadRequest,
                Type = "https://httpstatuses.com/400"
            },

            "NOT_FOUND" => new ProblemDetails
            {
                Title = "Resource not found",
                Detail = GetFirstError(errors),
                Status = StatusCodes.Status404NotFound,
                Type = "https://httpstatuses.com/404"
            },

            "CONFLICT" => new ProblemDetails
            {
                Title = "Conflict",
                Detail = GetFirstError(errors),
                Status = StatusCodes.Status409Conflict,
                Type = "https://httpstatuses.com/409"
            },

            _ => new ProblemDetails
            {
                Title = "Unexpected error",
                Detail = GetFirstError(errors),
                Status = StatusCodes.Status500InternalServerError,
                Type = "https://httpstatuses.com/500"
            }
        };
    }

    private static string GetFirstError(Dictionary<string, string[]> errors)
    {
        return errors.Values.FirstOrDefault()?.FirstOrDefault() ?? "Erro desconhecido";
    }
}
