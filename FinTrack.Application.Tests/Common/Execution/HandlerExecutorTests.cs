using FinTrack.Application.Common.Execution;
using FinTrack.Application.Common.Results;
using FluentAssertions;
using Moq;

namespace FinTrack.Application.Tests.Common.Execution;

public class HandlerExecutorTests
{
    public class FakeRequest { }

    [Fact]
    public async Task Execute_ShouldReturnFailure_WhenStepReturnsFailure()
    {
        // Arrange
        var request = new FakeRequest();

        var step = new Mock<IExecutionStep>();

        step.Setup(s => s.Execute(
                request,
                It.IsAny<CancellationToken>(),
                It.IsAny<Func<Task<Result<object>>>>()))
            .ReturnsAsync(Result<object>.Failure(
                new Dictionary<string, string[]>
                {
                { "Name", ["Name is required"] }
                },
                "ValidationError"));

        var executor = new HandlerExecutor([step.Object]);

        var handlerCalled = false;

        Task<Result<object>> Handler()
        {
            handlerCalled = true;
            return Task.FromResult(Result<object>.Success("ok"));
        }

        // Act
        var result = await executor.Execute(
            request,
            Handler,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainKey("Name");

        handlerCalled.Should().BeFalse();
    }

    [Fact]
    public async Task Execute_ShouldCallHandler_WhenStepCallsNext()
    {
        // Arrange
        var request = new FakeRequest();

        var step = new Mock<IExecutionStep>();

        step.Setup(s => s.Execute(
                request,
                It.IsAny<CancellationToken>(),
                It.IsAny<Func<Task<Result<object>>>>()))
            .Returns((object req, CancellationToken ct, Func<Task<Result<object>>> next)
                => next());

        var executor = new HandlerExecutor([step.Object]);

        var handlerCalled = false;

        Task<Result<object>> Handler()
        {
            handlerCalled = true;
            return Task.FromResult(Result<object>.Success("ok"));
        }

        // Act
        var result = await executor.Execute(
            request,
            Handler,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("ok");

        handlerCalled.Should().BeTrue();
    }

    [Fact]
    public async Task Execute_ShouldCallHandler_WhenNoSteps()
    {
        // Arrange
        var request = new FakeRequest();

        var executor = new HandlerExecutor([]);

        var handlerCalled = false;

        Task<Result<object>> Handler()
        {
            handlerCalled = true;
            return Task.FromResult(Result<object>.Success("ok"));
        }

        // Act
        var result = await executor.Execute(
            request,
            Handler,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("ok");

        handlerCalled.Should().BeTrue();
    }
}