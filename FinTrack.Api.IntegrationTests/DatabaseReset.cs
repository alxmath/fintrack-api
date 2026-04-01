using Npgsql;
using Respawn;

namespace FinTrack.Api.IntegrationTests;


public class DatabaseReset
{
    private readonly string _connectionString;
    private Respawner _respawner = default!;

    public DatabaseReset(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task InitializeAsync()
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        _respawner = await Respawner.CreateAsync(conn, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["public"]
        });
    }

    public async Task ResetAsync()
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        await _respawner.ResetAsync(conn);
    }
}
