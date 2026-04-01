using FinTrack.Application.Transactions.Create;
using FinTrack.Application.Transactions.Get;
using FluentAssertions;
using System.Net.Http.Json;

namespace FinTrack.Api.IntegrationTests.Transactions;

[Collection("IntegrationTests")]
public class CreateTransactionTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public CreateTransactionTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Post_ShouldCreateTransaction()
    {
        // Arrange
        var request = new CreateTransactionCommand(
            "Salário",
            1000,
            DateTime.UtcNow.AddSeconds(-1)
        );

        // Act
        var postResponse = await _client.PostAsJsonAsync(
            "/api/v1/transactions", request);

        // Assert (POST)
        postResponse.EnsureSuccessStatusCode();

        // Assert (persistência real)
        var getResponse = await _client.GetAsync("/api/v1/transactions");

        getResponse.EnsureSuccessStatusCode();

        var content = await getResponse.Content
            .ReadFromJsonAsync<List<GetTransactionsResponse>>();

        content.Should().ContainSingle(x =>
            x.Description == "Salário" &&
            x.Amount == 1000
        );
    }
}