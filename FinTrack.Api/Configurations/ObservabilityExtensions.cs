using FinTrack.Application.Common.Observability;
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

                    .SetSampler(new AlwaysOnSampler()) // env DEV!
                    //.SetSampler(new TraceIdRatioBasedSampler(0.1)) // env PROD!

                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.RecordException = true;
                    })

                    .AddHttpClientInstrumentation()

                    .AddSource(ActivitySources.Application)
                    .AddSource(ActivitySources.Infrastructure)

                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri("http://localhost:4317");
                        options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;

                        //options.ExportProcessorType = ExportProcessorType.Simple; // env DEV!
                        options.ExportProcessorType = ExportProcessorType.Batch; // env PROD!
                    })

                    .AddConsoleExporter(); // manter pra debug
            });

        return services;
    }
}