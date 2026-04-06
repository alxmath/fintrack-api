using FinTrack.Application.Common.Observability;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using System.Diagnostics;

namespace FinTrack.Infrastructure.Observability.Interceptors;

public class TracingDbCommandInterceptor : DbCommandInterceptor
{
    private static readonly ActivitySource Source = ActivitySources.InfrastructureSource;

    public override async ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = default)
    {
        var activity = Source.StartActivity("DB Query", ActivityKind.Client);

        Enrich(activity, command);

        try
        {
            return await base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
        }
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error);
            activity?.SetTag("error", true);
            activity?.SetTag("error.message", ex.Message);
            throw;
        }
        finally
        {
            activity?.Dispose();
        }
    }

    private static void Enrich(Activity? activity, DbCommand command)
    {
        if (activity is null) return;

        activity.SetTag("db.system", "postgresql");

        var operation = command.CommandText?.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
        activity.SetTag("db.operation", operation);

        activity.SetTag("db.name", command.Connection?.Database);
    }
}