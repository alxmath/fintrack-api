using FinTrack.Application.Features.Categories.Create;
using FluentAssertions;
using System.Net.Http.Json;

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

    protected async Task<Guid> CreateCategoryAsync(string name = "Default")
    {
        var request = new CreateCategoryCommand(name);

        var response = await Client.PostAsJsonAsync(
            "/api/v1/categories", request);

        response.EnsureSuccessStatusCode();

        var result = await response.Content
            .ReadFromJsonAsync<CreateCategoryResponse>();

        result.Should().NotBeNull();

        return result!.Id;
    }

    public async Task InitializeAsync()
    {
        await _reset.InitializeAsync();
        await _reset.ResetAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;
}
