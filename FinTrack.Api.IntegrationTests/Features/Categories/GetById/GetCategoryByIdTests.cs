using FinTrack.Api.IntegrationTests.Infrastructure;
using FinTrack.Application.Features.Categories.Create;
using FinTrack.Application.Features.Categories.GetById;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;

namespace FinTrack.Api.IntegrationTests.Features.Categories.GetById;

[Collection("IntegrationTests")]
public class GetCategoryByIdTests : IntegrationTestBase
{
    public GetCategoryByIdTests(PostgreSqlContainerFixture fixture)
        : base(fixture) { }

    [Fact]
    public async Task GetById_ShouldReturnCategory_WhenExists()
    {
        // Arrange
        var createResponse = await Client.PostAsJsonAsync(
            "/api/v1/categories",
            new CreateCategoryCommand("Transporte"));

        var created = await createResponse.Content
            .ReadFromJsonAsync<CreateCategoryResponse>();

        created.Should().NotBeNull();
        created.Id.Should().NotBeEmpty();

        // Act
        var response = await Client.GetAsync($"/api/v1/categories/{created.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<GetCategoryByIdResponse>();

        content.Should().NotBeNull();
        content!.Id.Should().Be(created.Id);
        content.Name.Should().Be("Transporte");
    }

    [Fact]
    public async Task GetById_ShouldReturn404_WhenCategoryDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var response = await Client.GetAsync($"/api/v1/categories/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        problem.Should().NotBeNull();
        problem!.Status.Should().Be(404);
        problem.Title.Should().Be("Resource not found");
    }

    [Fact]
    public async Task Post_Then_GetById_ShouldReturnCreatedCategory()
    {
        // Arrange
        var command = new CreateCategoryCommand("Saúde");

        // Act (POST)
        var postResponse = await Client.PostAsJsonAsync("/api/v1/categories", command);

        postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await postResponse.Content.ReadFromJsonAsync<CreateCategoryResponse>();

        // Act (GET)
        var getResponse = await Client.GetAsync($"/api/v1/categories/{created!.Id}");

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var fetched = await getResponse.Content.ReadFromJsonAsync<GetCategoryByIdResponse>();

        fetched!.Id.Should().Be(created.Id);
        fetched.Name.Should().Be("Saúde");
    }
}
