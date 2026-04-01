using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Features.Transactions.Create;
using FinTrack.Domain.Entities;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace FinTrack.Application.Tests.Transactions.Create;

public class CreateTransactionHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateTransaction_WhenCommandIsValid()
    {
        // Arrange
        var repositoryMock = new Mock<ITransactionRepository>();
        var validatorMock = new Mock<IValidator<CreateTransactionCommand>>();

        var command = new CreateTransactionCommand(
            "Salário",
            1000,
            DateTime.UtcNow,
            Guid.NewGuid()
        );

        validatorMock
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new CreateTransactionHandler(
            repositoryMock.Object,
            validatorMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Description.Should().Be("Salário");

        repositoryMock.Verify(
            r => r.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenValidationFails()
    {
        // Arrange
        var repositoryMock = new Mock<ITransactionRepository>();
        var validatorMock = new Mock<IValidator<CreateTransactionCommand>>();

        var command = new CreateTransactionCommand(
            "",
            0,
            DateTime.UtcNow.AddDays(1),
            Guid.NewGuid()
        );

        var validationFailures = new List<ValidationFailure>
    {
        new("Description", "Descrição é obrigatória")
    };

        validatorMock
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(validationFailures));

        var handler = new CreateTransactionHandler(
            repositoryMock.Object,
            validatorMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Descrição é obrigatória");

        repositoryMock.Verify(
            r => r.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldNotPersist_WhenValidationFails()
    {
        var repositoryMock = new Mock<ITransactionRepository>();
        var validatorMock = new Mock<IValidator<CreateTransactionCommand>>();

        var command = new CreateTransactionCommand("", 0, DateTime.UtcNow, Guid.NewGuid());

        validatorMock
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(
                [new ValidationFailure("Description", "Erro")]));

        var handler = new CreateTransactionHandler(
            repositoryMock.Object,
            validatorMock.Object);

        await handler.Handle(command, CancellationToken.None);

        repositoryMock.Verify(
            r => r.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
