using FinTrack.Application.Categories.Create;
using FinTrack.Application.Common.Interfaces;
using FinTrack.Domain.Entities;
using FluentAssertions;
using FluentValidation;
using Moq;

namespace FinTrack.Application.Tests.Categories.Create;

public class CreateCategoryHandlerTests
{
    private readonly Mock<ICategoryRepository> _repositoryMock = new();
    private readonly Mock<IValidator<CreateCategoryCommand>> _validatorMock = new();

    private readonly CreateCategoryHandler _handler;

    public CreateCategoryHandlerTests()
    {
        _handler = new CreateCategoryHandler(
            _repositoryMock.Object,
            _validatorMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenValidationFails()
    {
        // Arrange
        var command = new CreateCategoryCommand("");

        var validationResult = new FluentValidation.Results.ValidationResult(
            new[] { new FluentValidation.Results.ValidationFailure("Name", "Nome inválido") }
        );

        _validatorMock
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Nome inválido");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCategoryAlreadyExists()
    {
        // Arrange
        var command = new CreateCategoryCommand("Alimentação");

        _validatorMock
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _repositoryMock
            .Setup(r => r.ExistsByNameAsync(command.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Categoria já existe");
    }

    [Fact]
    public async Task Handle_ShouldCreateCategory_WhenValid()
    {
        // Arrange
        var command = new CreateCategoryCommand("Alimentação");

        _validatorMock
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _repositoryMock
            .Setup(r => r.ExistsByNameAsync(command.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Nome.Should().Be("Alimentação");

        _repositoryMock.Verify(r =>
            r.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
