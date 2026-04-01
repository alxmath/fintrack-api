using FinTrack.Application.Transactions.Create;
using FinTrack.Application.Transactions.Get;
using FluentAssertions;
using System.Net.Http.Json;

namespace FinTrack.Api.IntegrationTests.Transactions;

[Collection("IntegrationTests")]
public class CreateTransactionTests
{
    private readonly HttpClient _client;
    private readonly DatabaseReset _reset;

    public CreateTransactionTests(PostgreSqlContainerFixture fixture)
    {
        var factory = new CustomWebApplicationFactory(fixture.ConnectionString);
        _client = factory.CreateClient();

        _reset = new DatabaseReset(fixture.ConnectionString);
        _reset.InitializeAsync().GetAwaiter().GetResult();
    }

    [Fact]
    public async Task Post_ShouldCreateTransaction()
    {
        // Arrange
        await _reset.ResetAsync();

        var request = new CreateTransactionCommand(
            "Salário",
            1000,
            DateTime.UtcNow.AddSeconds(-1)
        );

        var postResponse = await _client.PostAsJsonAsync(
            "/api/v1/transactions", request);

        postResponse.EnsureSuccessStatusCode();

        // Act
        var getResponse = await _client.GetAsync("/api/v1/transactions");

        // Assert
        var content = await getResponse.Content
            .ReadFromJsonAsync<List<GetTransactionsResponse>>();

        content.Should().ContainSingle(x =>
            x.Description == "Salário" &&
            x.Amount == 1000
        );
    }
}