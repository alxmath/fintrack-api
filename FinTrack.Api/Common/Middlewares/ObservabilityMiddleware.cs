using FinTrack.Application.Common.Observability;
using System.Diagnostics;

namespace FinTrack.Api.Common.Middlewares;

public class ObservabilityMiddleware
{
    private readonly RequestDelegate _next;

    public ObservabilityMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var path = context.Request.Path;

        using var activity = ActivitySources.ApplicationSource
            .StartActivity($"HTTP {context.Request.Method} {path}", ActivityKind.Server);

        activity?.SetTag("http.method", context.Request.Method);
        activity?.SetTag("http.route", path);

        await _next(context);

        activity?.SetTag("http.status_code", context.Response.StatusCode);
    }
}
