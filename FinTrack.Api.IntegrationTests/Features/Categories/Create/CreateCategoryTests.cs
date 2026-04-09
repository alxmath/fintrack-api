using FinTrack.Api.IntegrationTests.Infrastructure;
using FinTrack.Application.Features.Categories.Create;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;

namespace FinTrack.Api.IntegrationTests.Features.Categories.Create;

[Collection("IntegrationTests")]
public class CreateCategoryTests : IntegrationTestBase
{
    public CreateCategoryTests(PostgreSqlContainerFixture fixture)
        : base(fixture) { }

    [Fact]
    public async Task Post_ShouldCreateCategory()
    {
        // Arrange
        await AuthHelper.AuthenticateAsync(Client);

        var request = new CreateCategoryCommand("Alimentação");

        // Act
        var response = await Client.PostAsJsonAsync(
            "/api/v1/categories", request);

        var raw = await response.Content.ReadAsStringAsync();
        Console.WriteLine(raw);

        // Assert
        response.EnsureSuccessStatusCode();

        var result = await response.Content
            .ReadFromJsonAsync<CreateCategoryResponse>();

        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        result.Name.Should().Be("Alimentação");
    }

    [Fact]
    public async Task Post_ShouldReturnError_WhenCategoryAlreadyExists()
    {
        // Arrange
        await AuthHelper.AuthenticateAsync(Client);

        var request = new CreateCategoryCommand("Alimentação");

        await Client.PostAsJsonAsync("/api/v1/categories", request);

        // Act
        var response = await Client.PostAsJsonAsync(
            "/api/v1/categories", request);

        // Assert
        var problem = await response.Content
            .ReadFromJsonAsync<ProblemDetails>();

        problem.Should().NotBeNull();
        problem!.Title.Should().Be("Conflict");
        problem.Status.Should().Be(409);
        problem.Detail.Should().Contain("Categoria já existe");
    }

    [Fact]
    public async Task Post_ShouldReturn401_WhenNotAuthenticated()
    {
        // Arrange
        var request = new CreateCategoryCommand("Teste");

        // Act
        var response = await Client.PostAsJsonAsync("/api/v1/categories", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
