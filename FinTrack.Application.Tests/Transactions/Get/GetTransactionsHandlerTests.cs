using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Transactions.Get;
using FinTrack.Domain.Entities;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace FinTrack.Application.Tests.Transactions.Get;

public class GetTransactionsHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnTransactions_WhenDataExists()
    {
        // Arrange
        var repositoryMock = new Mock<ITransactionRepository>();
        var validatorMock = new Mock<IValidator<GetTransactionsQuery>>();

        var transactions = new List<Transaction>
        {
            new("Salário", 100, DateTime.UtcNow,
            Guid.NewGuid()),
            new("Mercado", 50, DateTime.UtcNow.AddDays(-1),
            Guid.NewGuid())
        };

        repositoryMock
            .Setup(r => r.GetPagedAsync(1, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(transactions);

        validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<GetTransactionsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var handler = new GetTransactionsHandler(
            repositoryMock.Object,
            validatorMock.Object);

        var query = new GetTransactionsQuery(1, 10);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
        result.Value.Should().Contain(x => x.Description == "Salário");
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoData()
    {
        // Arrange
        var repositoryMock = new Mock<ITransactionRepository>();
        var validatorMock = new Mock<IValidator<GetTransactionsQuery>>();

        repositoryMock
            .Setup(r => r.GetPagedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Transaction>());

        validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<GetTransactionsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var handler = new GetTransactionsHandler(
            repositoryMock.Object,
            validatorMock.Object);

        var query = new GetTransactionsQuery(1, 10);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldCallRepository_WithCorrectParameters()
    {
        // Arrange
        var repositoryMock = new Mock<ITransactionRepository>();
        var validatorMock = new Mock<IValidator<GetTransactionsQuery>>();

        validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<GetTransactionsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var handler = new GetTransactionsHandler(
            repositoryMock.Object,
            validatorMock.Object);

        var query = new GetTransactionsQuery(2, 5);

        repositoryMock
            .Setup(r => r.GetPagedAsync(2, 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Transaction>());

        // Act
        await handler.Handle(query, CancellationToken.None);

        // Assert
        repositoryMock.Verify(
            r => r.GetPagedAsync(2, 5, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenValidationFails()
    {
        // Arrange
        var repositoryMock = new Mock<ITransactionRepository>();
        var validatorMock = new Mock<IValidator<GetTransactionsQuery>>();

        var failures = new List<ValidationFailure>
    {
        new("PageNumber", "Inválido")
    };

        validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<GetTransactionsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(failures));

        var handler = new GetTransactionsHandler(
            repositoryMock.Object,
            validatorMock.Object);

        var query = new GetTransactionsQuery(0, 10);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();

        repositoryMock.Verify(
            r => r.GetPagedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}