using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Results;
using FinTrack.Application.Features.Transactions.Create;
using FinTrack.Domain.Entities;
using FluentAssertions;
using Moq;

namespace FinTrack.Application.Tests.Features.Transactions.Create;

public class CreateTransactionHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateTransaction_WhenCommandIsValid()
    {
        // Arrange
        var transactionRepositoryMock = new Mock<ITransactionRepository>();
        var categoryRespositoryMock = new Mock<ICategoryRepository>();
        var userContextMock = new Mock<IUserContext>();

        var command = new CreateTransactionCommand(
            "Salário",
            1000,
            DateTime.UtcNow,
            Guid.NewGuid()
        );

        var category = new Category("Test Category", Guid.NewGuid());

        transactionRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        categoryRespositoryMock
            .Setup(c => c.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        userContextMock.Setup(x => x.UserId).Returns(Guid.NewGuid());

        var handler = new CreateTransactionHandler(
            transactionRepositoryMock.Object,
            categoryRespositoryMock.Object,
            userContextMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Description.Should().Be("Salário");

        transactionRepositoryMock.Verify(
            r => r.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenCategoryDoesNotExist()
    {
        // Arrange
        var transactionRepositoryMock = new Mock<ITransactionRepository>();
        var categoryRespositoryMock = new Mock<ICategoryRepository>();
        var userContextMock = new Mock<IUserContext>();

        var command = new CreateTransactionCommand(
            "Salário",
            1000,
            DateTime.UtcNow,
            Guid.NewGuid()
        );

        userContextMock.Setup(x => x.UserId).Returns(Guid.NewGuid());

        categoryRespositoryMock
            .Setup(c => c.GetByIdAsync(
                command.CategoryId,
                userContextMock.Object.UserId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Category);

        var handler = new CreateTransactionHandler(
        transactionRepositoryMock.Object,
        categoryRespositoryMock.Object,
        userContextMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
        result.ErrorCode.Should().Be("NOT_FOUND");
        result.Errors.Should().ContainKey("CategoryId");

        transactionRepositoryMock.Verify(
            r => r.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
