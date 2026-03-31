using FinTrack.Application.Common.Abstractions;
using FinTrack.Application.Transactions.Create;
using FluentAssertions;
using Moq;

namespace FinTrack.Application.Tests.Transactions.Create;

public class CreateTransactionValidatorTests
{
    [Fact]
    public void Validate_ShouldPass_WhenCommandIsValid()
    {
        // Arrange
        var dateTimeProviderMock = new Mock<IDateTimeProvider>();

        var fixedNow = new DateTime(2024, 01, 01);

        dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(fixedNow);

        var validator = new CreateTransactionValidator(dateTimeProviderMock.Object);

        var command = new CreateTransactionCommand(
            "Salário",
            1000,
            fixedNow // exatamente igual → válido
        );

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_ShouldFail_WhenDescriptionIsEmpty()
    {
        // Arrange
        var dateTimeProviderMock = new Mock<IDateTimeProvider>();

        var fixedNow = new DateTime(2024, 01, 01);

        dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(fixedNow);

        var validator = new CreateTransactionValidator(dateTimeProviderMock.Object);

        var command = new CreateTransactionCommand(
            "",
            100,
            fixedNow
        );

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Description");
    }

    [Fact]
    public void Validate_ShouldFail_WhenAmountIsZero()
    {
        // Arrange
        var dateTimeProviderMock = new Mock<IDateTimeProvider>();

        var fixedNow = new DateTime(2024, 01, 01);

        dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(fixedNow);

        var validator = new CreateTransactionValidator(dateTimeProviderMock.Object);

        var command = new CreateTransactionCommand(
            "Teste",
            0,
            fixedNow
        );

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Amount");
    }

    [Fact]
    public void Validate_ShouldFail_WhenDateIsFuture()
    {
        // Arrange
        var dateTimeProviderMock = new Mock<IDateTimeProvider>();

        var fixedNow = new DateTime(2024, 01, 01);

        dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(fixedNow);

        var validator = new CreateTransactionValidator(dateTimeProviderMock.Object);

        var command = new CreateTransactionCommand(
            "Teste",
            100,
            fixedNow.AddSeconds(1) // futuro
        );

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Date");
    }
}
