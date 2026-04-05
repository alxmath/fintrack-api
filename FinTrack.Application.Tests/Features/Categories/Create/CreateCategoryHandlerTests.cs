using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Features.Categories.Create;
using FinTrack.Domain.Entities;
using FluentAssertions;
using Moq;

namespace FinTrack.Application.Tests.Features.Categories.Create;

public class CreateCategoryHandlerTests
{
    private readonly Mock<ICategoryRepository> _repositoryMock = new();
    private readonly Mock<IUserContext> _userContextMock = new();

    private readonly CreateCategoryHandler _handler;

    public CreateCategoryHandlerTests()
    {
        _handler = new CreateCategoryHandler(
            _repositoryMock.Object,
            _userContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCategoryAlreadyExists()
    {
        // Arrange
        var command = new CreateCategoryCommand("Alimentação");

        _repositoryMock
            .Setup(r => r.ExistsByNameAsync(
                command.Name,
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _userContextMock
            .Setup(uc => uc.UserId)
            .Returns(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainKey("Name");
        result.Errors["Name"].Should().Contain("Categoria já existe");
    }

    [Fact]
    public async Task Handle_ShouldCreateCategory_WhenValid()
    {
        // Arrange
        var command = new CreateCategoryCommand("Alimentação");

        _repositoryMock
            .Setup(r => r.ExistsByNameAsync(
                command.Name,
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _userContextMock
            .Setup(uc => uc.UserId)
            .Returns(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Name.Should().Be("Alimentação");

        _repositoryMock.Verify(r =>
            r.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
