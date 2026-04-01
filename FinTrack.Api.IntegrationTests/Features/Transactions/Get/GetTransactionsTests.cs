using FinTrack.Api.IntegrationTests.Infrastructure;
using FinTrack.Application.Categories.Create;
using FinTrack.Application.Common.Results;
using FinTrack.Application.Transactions.Create;
using FinTrack.Application.Transactions.Get;
using FluentAssertions;
using System.Net.Http.Json;

namespace FinTrack.Api.IntegrationTests.Features.Transactions.Get;

[Collection("IntegrationTests")]
public class GetTransactionsTests : IntegrationTestBase
{
    public GetTransactionsTests(PostgreSqlContainerFixture fixture)
         : base(fixture) { }

    [Fact]
    public async Task Get_ShouldReturnCreatedTransaction()
    {
        // Arrange
        var categoryRequest = new CreateCategoryCommand("Salário");

        var categoryResponse = await Client.PostAsJsonAsync(
            "/api/v1/categories", categoryRequest);

        var categoryResult = await categoryResponse.Content
            .ReadFromJsonAsync<Result<CreateCategoryResponse>>();

        var categoryId = categoryResult?.Value?.Id ?? Guid.Empty;

        var request = new CreateTransactionCommand(
            "Salário",
            1000,
            DateTime.UtcNow,
            categoryId
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
