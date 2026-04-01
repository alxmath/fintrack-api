using FinTrack.Api.IntegrationTests.Infrastructure;
using FinTrack.Application.Categories.Create;
using FinTrack.Application.Common.Results;
using FluentAssertions;
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
        var request = new CreateCategoryCommand("Alimentação");

        // Act
        var response = await Client.PostAsJsonAsync(
            "/api/v1/categories", request);

        var raw = await response.Content.ReadAsStringAsync();
        Console.WriteLine(raw);

        // Assert
        response.EnsureSuccessStatusCode();

        var result = await response.Content
            .ReadFromJsonAsync<Result<CreateCategoryResponse>>();

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value?.Id.Should().NotBeEmpty();
        result.Value?.Nome.Should().Be("Alimentação");
    }

    [Fact]
    public async Task Post_ShouldReturnError_WhenCategoryAlreadyExists()
    {
        var request = new CreateCategoryCommand("Alimentação");

        await Client.PostAsJsonAsync("/api/v1/categories", request);

        var response = await Client.PostAsJsonAsync(
            "/api/v1/categories", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var result = await response.Content
            .ReadFromJsonAsync<Result<CreateCategoryResponse>>();

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Categoria já existe");
    }
}
