using FinTrack.Api.IntegrationTests.Infrastructure;
using FinTrack.Application.Common.Results;
using FinTrack.Application.Features.Transactions.Create;
using FinTrack.Application.Features.Transactions.GetById;
using FluentAssertions;
using System.Net.Http.Json;

namespace FinTrack.Api.IntegrationTests.Features.Transactions.GetById;

[Collection("IntegrationTests")]
public class GetTransactionByIdTests : IntegrationTestBase
{
    public GetTransactionByIdTests(PostgreSqlContainerFixture fixture)
         : base(fixture) { }

    [Fact]
    public async Task GetById_ShouldReturnTransaction_WhenExists()
    {
        // Arrange
        var categoryId = await CreateCategoryAsync("Test");

        var createResponse = await Client.PostAsJsonAsync(
            "/api/v1/transactions",
            new CreateTransactionCommand("Test", 100, DateTime.UtcNow, categoryId));

        var created = await createResponse.Content
            .ReadFromJsonAsync<Result<CreateTransactionResponse>>();

        var id = created!.Value.Id;

        // Act
        var response = await Client.GetAsync($"/api/v1/transactions/{id}");

        // Assert
        response.EnsureSuccessStatusCode();

        var result = await response.Content
            .ReadFromJsonAsync<Result<GetTransactionByIdResponse>>();

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Id.Should().Be(id);
    }
}
