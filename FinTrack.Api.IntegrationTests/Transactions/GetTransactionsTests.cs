using FinTrack.Application.Transactions.Create;
using FinTrack.Application.Transactions.Get;
using FluentAssertions;
using System.Net.Http.Json;

namespace FinTrack.Api.IntegrationTests.Transactions;

[Collection("IntegrationTests")]
public class GetTransactionsTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public GetTransactionsTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_ShouldReturnCreatedTransaction()
    {
        // Arrange (cria dado real)
        var request = new CreateTransactionCommand(
            "Salário",
            1000,
            DateTime.UtcNow
        );

        var postResponse = await _client.PostAsJsonAsync(
            "/api/v1/transactions", request);

        postResponse.EnsureSuccessStatusCode();

        // Act
        var response = await _client.GetAsync("/api/v1/transactions");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var content = await response.Content
            .ReadFromJsonAsync<List<GetTransactionsResponse>>();

        content.Should().NotBeNull();
        content.Should().ContainSingle(x =>
            x.Description == "Salário" &&
            x.Amount == 1000
        );
    }
}
