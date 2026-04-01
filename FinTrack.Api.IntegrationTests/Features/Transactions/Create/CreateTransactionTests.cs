using FinTrack.Api.IntegrationTests.Infrastructure;
using FinTrack.Application.Common.Results;
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
        var categoryId = await CreateCategoryAsync("Salário");

        var request = new CreateTransactionCommand(
            "Salário",
            1000,
            DateTime.UtcNow.AddSeconds(-1),
            categoryId 
        );

        var postResponse = await Client.PostAsJsonAsync(
            "/api/v1/transactions", request);

        postResponse.EnsureSuccessStatusCode();

        // Act
        var getResponse = await Client.GetAsync("/api/v1/transactions");

        // Assert
        var content = await getResponse.Content
            .ReadFromJsonAsync<Result<List<GetTransactionsResponse>>>();

        content.Should().NotBeNull();
        content.Value.Should().ContainSingle(x =>
            x.Description == "Salário" &&
            x.Amount == 1000
        );
    }
}