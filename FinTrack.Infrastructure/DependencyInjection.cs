using FinTrack.Application.Common.Abstractions;
using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Transactions.Create;
using FinTrack.Application.Transactions.Get;
using FinTrack.Infrastructure.Persistence;
using FinTrack.Infrastructure.Persistence.Repositories;
using FinTrack.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FinTrack.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<ITransactionRepository, TransactionRepository>();

        services.AddScoped<CreateTransactionHandler>();
        services.AddScoped<GetTransactionsHandler>();

        services.AddScoped<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}
