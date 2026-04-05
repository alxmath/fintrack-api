using FinTrack.Api.IntegrationTests.Infrastructure;
using FinTrack.Application.Features.Categories.Get;
using FluentAssertions;
using System.Net.Http.Json;

namespace FinTrack.Api.IntegrationTests.Features.Categories.Get;

[Collection("IntegrationTests")]
public class GetCategoriesTests : IntegrationTestBase
{
    public GetCategoriesTests(PostgreSqlContainerFixture fixture)
        : base(fixture) { }

    [Fact]
    public async Task Get_ShouldReturnEmptyList_WhenNoCategoriesExist()
    {
        // Act
        await AuthHelper.AuthenticateAsync(Client);

        var response = await Client.GetAsync("/api/v1/categories");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var result = await response.Content
            .ReadFromJsonAsync<List<GetCategoriesResponse>>();

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Get_ShouldReturnCategories_WhenTheyExist()
    {
        // Arrange
        await AuthHelper.AuthenticateAsync(Client);

        await CreateCategoryAsync("Alimentação");
        await CreateCategoryAsync("Salário");

        // Act
        var response = await Client.GetAsync("/api/v1/categories");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var result = await response.Content
            .ReadFromJsonAsync<List<GetCategoriesResponse>>();

        result.Should().NotBeNull();

        result.Should().HaveCount(2);
        result.Should().Contain(c => c.Name == "Alimentação");
        result.Should().Contain(c => c.Name == "Salário");
    }
}