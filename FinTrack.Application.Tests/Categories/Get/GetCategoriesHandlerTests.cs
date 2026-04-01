using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Features.Categories.Get;
using FinTrack.Domain.Entities;
using FluentAssertions;
using Moq;

namespace FinTrack.Application.Tests.Categories.Get;

public class GetCategoriesHandlerTests
{
    private readonly Mock<ICategoryRepository> _repositoryMock;
    private readonly GetCategoriesHandler _handler;

    public GetCategoriesHandlerTests()
    {
        _repositoryMock = new Mock<ICategoryRepository>();
        _handler = new GetCategoriesHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoCategoriesExist()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var query = new GetCategoriesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldReturnCategories_WhenTheyExist()
    {
        // Arrange
        var categories = new List<Category>
        {
            new("Alimentação"),
            new("Salário")
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(categories);

        var query = new GetCategoriesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
        result.Value.Should().Contain(c => c.Name == "Alimentação");
        result.Value.Should().Contain(c => c.Name == "Salário");
    }
}