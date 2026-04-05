using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace FinTrack.Api.Configurations;

public static class ObservabilityExtensions
{
    public static IServiceCollection AddObservability(
        this IServiceCollection services)
    {
        services.AddOpenTelemetry()
            .WithTracing(tracing =>
            {
                tracing
                    .SetResourceBuilder(ResourceBuilder.CreateDefault()
                        .AddService("FinTrack.Api"))

                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.RecordException = true;
                    })

                    .AddHttpClientInstrumentation()

                    .AddSource("FinTrack.Application")

                    .AddConsoleExporter();
            });

        return services;
    }
}