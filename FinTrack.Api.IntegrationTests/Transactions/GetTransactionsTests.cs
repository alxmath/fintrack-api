using FinTrack.Application.Transactions.Create;
using FinTrack.Application.Transactions.Get;
using FluentAssertions;
using System.Net.Http.Json;

namespace FinTrack.Api.IntegrationTests.Transactions;

[Collection("IntegrationTests")]
public class GetTransactionsTests : IntegrationTestBase
{
    public GetTransactionsTests(PostgreSqlContainerFixture fixture)
         : base(fixture) { }

    [Fact]
    public async Task Get_ShouldReturnCreatedTransaction()
    {
        // Arrange
        var request = new CreateTransactionCommand(
            "Salário",
            1000,
            DateTime.UtcNow
        );

        var postResponse = await Client.PostAsJsonAsync(
            "/api/v1/transactions", request);

        postResponse.EnsureSuccessStatusCode();

        // Act
        var response = await Client.GetAsync("/api/v1/transactions");

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
