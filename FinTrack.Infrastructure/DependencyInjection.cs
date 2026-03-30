using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Transactions.Create;
using FinTrack.Infrastructure.Persistence;
using FinTrack.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FinTrack.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraestructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<ITransactionRepository, TransactionRepository>();

        services.AddScoped<CreateTransactionHandler>();

        return services;
    }
}
