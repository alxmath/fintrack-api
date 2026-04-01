using FinTrack.Api.IntegrationTests.Infrastructure;
using FinTrack.Application.Transactions.Create;
using FinTrack.Application.Transactions.Get;
using FluentAssertions;
using System.Net.Http.Json;

namespace FinTrack.Api.IntegrationTests.Features.Transactions.Create;

[Collection("IntegrationTests")]
public class CreateTransactionTests : IntegrationTestBase
{
    public CreateTransactionTests(PostgreSqlContainerFixture fixture) 
        : base(fixture) { }

    [Fact]
    public async Task Post_ShouldCreateTransaction()
    {
        // Arrange
        var request = new CreateTransactionCommand(
            "Salário",
            1000,
            DateTime.UtcNow.AddSeconds(-1)
        );

        var postResponse = await Client.PostAsJsonAsync(
            "/api/v1/transactions", request);

        postResponse.EnsureSuccessStatusCode();

        // Act
        var getResponse = await Client.GetAsync("/api/v1/transactions");

        // Assert
        var content = await getResponse.Content
            .ReadFromJsonAsync<List<GetTransactionsResponse>>();

        content.Should().ContainSingle(x =>
            x.Description == "Salário" &&
            x.Amount == 1000
        );
    }
}