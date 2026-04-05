using OpenTelemetry;
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

                    .SetSampler(new AlwaysOnSampler()) // ESSENCIAL

                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.RecordException = true;
                    })

                    .AddHttpClientInstrumentation()

                    .AddSource("FinTrack.Application")

                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri("http://localhost:4317");
                        options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;

                        options.ExportProcessorType = ExportProcessorType.Simple;
                    })

                    .AddConsoleExporter(); // manter pra debug
            });

        return services;
    }
}