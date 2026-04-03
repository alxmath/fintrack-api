using FinTrack.Application.Common.Interfaces;
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
        var repositoryMock = new Mock<ITransactionRepository>();

        var command = new CreateTransactionCommand(
            "Salário",
            1000,
            DateTime.UtcNow,
            Guid.NewGuid()
        );

        repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new CreateTransactionHandler(
            repositoryMock.Object);

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
}
