namespace FinTrack.Api.IntegrationTests.Infrastructure;

public abstract class IntegrationTestBase : IAsyncLifetime
{
    protected readonly HttpClient Client;
    private readonly DatabaseReset _reset;

    protected IntegrationTestBase(PostgreSqlContainerFixture fixture)
    {
        var factory = new CustomWebApplicationFactory(fixture.ConnectionString);
        Client = factory.CreateClient();

        _reset = new DatabaseReset(fixture.ConnectionString);
    }

    public async Task InitializeAsync()
    {
        await _reset.InitializeAsync();
        await _reset.ResetAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;
}
