using FinTrack.Api.IntegrationTests.Infrastructure;
using FinTrack.Application.Common.Results;
using FinTrack.Application.Features.Transactions.Create;
using FinTrack.Application.Features.Transactions.Get;
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
        await AuthHelper.AuthenticateAsync(Client);

        var categoryId = await CreateCategoryAsync("Salário");

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
            .ReadFromJsonAsync<PagedResult<GetTransactionsResponse>>();

        content.Should().NotBeNull();
        content.Items.Should().ContainSingle(x =>
            x.Description == "Salário" &&
            x.Amount == 1000
        );
    }

    [Fact]
    public async Task Get_ShouldOrderByAmountAscending()
    {
        // Arrange
        await AuthHelper.AuthenticateAsync(Client);

        var categoryId = await CreateCategoryAsync("Test");

        await Client.PostAsJsonAsync("/api/v1/transactions",
            new CreateTransactionCommand("T1", 300, DateTime.UtcNow.AddSeconds(-3), categoryId));

        await Client.PostAsJsonAsync("/api/v1/transactions",
            new CreateTransactionCommand("T2", 100, DateTime.UtcNow.AddSeconds(-2), categoryId));

        await Client.PostAsJsonAsync("/api/v1/transactions",
            new CreateTransactionCommand("T3", 200, DateTime.UtcNow.AddSeconds(-1), categoryId));

        // Act
        var response = await Client.GetAsync("/api/v1/transactions?orderBy=amount&desc=false");

        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content
            .ReadFromJsonAsync<PagedResult<GetTransactionsResponse>>();

        content.Should().NotBeNull();
        content.Items.Should().NotBeNull();

        var items = content.Items;

        items.Should().HaveCount(3);

        items[0].Amount.Should().Be(100);
        items[1].Amount.Should().Be(200);
        items[2].Amount.Should().Be(300);
    }

    [Fact]
    public async Task Get_ShouldOrderByDateDescendingByDefault()
    {
        // Arrange
        await AuthHelper.AuthenticateAsync(Client);

        var categoryId = await CreateCategoryAsync("Test");

        var older = DateTime.UtcNow.AddMinutes(-3);
        var middle = DateTime.UtcNow.AddMinutes(-2);
        var newer = DateTime.UtcNow.AddMinutes(-1);

        await Client.PostAsJsonAsync("/api/v1/transactions",
            new CreateTransactionCommand("Old", 100, older, categoryId));

        await Client.PostAsJsonAsync("/api/v1/transactions",
            new CreateTransactionCommand("Mid", 200, middle, categoryId));

        await Client.PostAsJsonAsync("/api/v1/transactions",
            new CreateTransactionCommand("New", 300, newer, categoryId));

        // Act
        var response = await Client.GetAsync("/api/v1/transactions");

        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content
            .ReadFromJsonAsync<PagedResult<GetTransactionsResponse>>();

        content.Should().NotBeNull();
        content.Items.Should().NotBeNull();

        var items = content.Items;

        items.Should().HaveCount(3);

        items[0].Description.Should().Be("New");
        items[1].Description.Should().Be("Mid");
        items[2].Description.Should().Be("Old");
    }

    [Fact]
    public async Task Get_ShouldFallbackToDateDescending_WhenOrderByIsInvalid()
    {
        // Arrange
        await AuthHelper.AuthenticateAsync(Client);

        var categoryId = await CreateCategoryAsync("Test");

        var older = DateTime.UtcNow.AddMinutes(-3);
        var middle = DateTime.UtcNow.AddMinutes(-2);
        var newer = DateTime.UtcNow.AddMinutes(-1);

        await Client.PostAsJsonAsync("/api/v1/transactions",
            new CreateTransactionCommand("Old", 100, older, categoryId));

        await Client.PostAsJsonAsync("/api/v1/transactions",
            new CreateTransactionCommand("Mid", 200, middle, categoryId));

        await Client.PostAsJsonAsync("/api/v1/transactions",
            new CreateTransactionCommand("New", 300, newer, categoryId));

        // Act
        var response = await Client.GetAsync("/api/v1/transactions?orderBy=invalido");

        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content
            .ReadFromJsonAsync<PagedResult<GetTransactionsResponse>>();

        content.Should().NotBeNull();
        content.Items.Should().NotBeNull();

        var items = content.Items;

        items.Should().HaveCount(3);

        // fallback esperado: Date DESC
        items[0].Description.Should().Be("New");
        items[1].Description.Should().Be("Mid");
        items[2].Description.Should().Be("Old");
    }

    [Fact]
    public async Task Get_ShouldReturnCategoryData()
    {
        // Arrange
        await AuthHelper.AuthenticateAsync(Client);

        var categoryId = await CreateCategoryAsync("Alimentação");

        await Client.PostAsJsonAsync("/api/v1/transactions",
            new CreateTransactionCommand("Mercado", 50, DateTime.UtcNow, categoryId));

        // Act
        var response = await Client.GetAsync("/api/v1/transactions");

        // Assert
        var content = await response.Content
            .ReadFromJsonAsync<PagedResult<GetTransactionsResponse>>();

        content.Should().NotBeNull();
        content.Items.Should().NotBeNull();
        content.Items.Should().ContainSingle(x =>
            x.Category.Name == "Alimentação"
        );
    }
}
