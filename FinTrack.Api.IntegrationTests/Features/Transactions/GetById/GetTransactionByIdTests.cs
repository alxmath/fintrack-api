using FinTrack.Api.IntegrationTests.Infrastructure;
using FinTrack.Application.Features.Transactions.Create;
using FinTrack.Application.Features.Transactions.GetById;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
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
        await AuthHelper.AuthenticateAsync(Client);

        var categoryId = await CreateCategoryAsync("Test");

        var createResponse = await Client.PostAsJsonAsync(
            "/api/v1/transactions",
            new CreateTransactionCommand("Test", 100, DateTime.UtcNow, categoryId));

        var created = await createResponse.Content
            .ReadFromJsonAsync<CreateTransactionResponse>();

        created.Should().NotBeNull();
        created.Id.Should().NotBeEmpty();

        var id = created.Id;

        // Act
        var response = await Client.GetAsync($"/api/v1/transactions/{id}");

        // Assert
        response.EnsureSuccessStatusCode();

        var result = await response.Content
            .ReadFromJsonAsync<GetTransactionByIdResponse>();

        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        result.Id.Should().Be(id);
    }

    [Fact]
    public async Task GetById_ShouldReturn404_WhenTransactionDoesNotExist()
    {
        // Arrange
        await AuthHelper.AuthenticateAsync(Client);

        var id = Guid.NewGuid();

        // Act
        var response = await Client.GetAsync($"/api/v1/transactions/{id}");

        // Assert
        //response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problem = await response.Content
            .ReadFromJsonAsync<ProblemDetails>();

        problem.Should().NotBeNull();
        problem!.Title.Should().Be("Resource not found");
        problem.Status.Should().Be(404);
        problem.Detail.Should().Contain("Transação não encontrada");
    }
}
