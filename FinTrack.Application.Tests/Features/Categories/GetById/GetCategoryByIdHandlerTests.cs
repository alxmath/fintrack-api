using FinTrack.Application.Common.Errors;
using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Features.Categories.GetById;
using FinTrack.Domain.Entities;
using FluentAssertions;
using Moq;

namespace FinTrack.Application.Tests.Features.Categories.GetById;

public class GetCategoryByIdHandlerTests
{
    private readonly Mock<ICategoryRepository> _repositoryMock;
    private readonly GetCategoryByIdHandler _handler;

    public GetCategoryByIdHandlerTests()
    {
        _repositoryMock = new Mock<ICategoryRepository>();
        _handler = new GetCategoryByIdHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCategory_WhenCategoryExists()
    {
        // Arrange
        var id = Guid.NewGuid();

        var category = new Category("Alimentação");
        typeof(Category).GetProperty(nameof(Category.Id))!
            .SetValue(category, id);

        _repositoryMock
            .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        var query = new GetCategoryByIdQuery(id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Id.Should().Be(id);
        result.Value.Name.Should().Be("Alimentação");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCategoryDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category?)null);

        var query = new GetCategoryByIdQuery(id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainKey("Name");
        result.Errors["Name"].Should().Contain("Categoria não encontrada");
        result.ErrorCode.Should().Be(Errors.General.NotFound);
    }
}
