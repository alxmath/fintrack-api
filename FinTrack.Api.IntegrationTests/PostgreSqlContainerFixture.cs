using FinTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace FinTrack.Api.IntegrationTests;

public class PostgreSqlContainerFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container;

    public string ConnectionString => _container.GetConnectionString();

    public PostgreSqlContainerFixture()
    {
        _container = new PostgreSqlBuilder("postgres:15")
           .WithDatabase("fintrack_test")
           .WithUsername("postgres")
           .WithPassword("postgres")
           .WithCleanUp(true)
           .Build();
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        await _container.StartAsync();

        // 🔥 MIGRATE UMA ÚNICA VEZ
        var services = new ServiceCollection();

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(ConnectionString));

        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }
}