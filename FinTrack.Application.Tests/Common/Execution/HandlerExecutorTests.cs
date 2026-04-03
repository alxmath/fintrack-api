using FinTrack.Application.Common.Execution;
using FinTrack.Application.Common.Results;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace FinTrack.Application.Tests.Common.Execution;

public class HandlerExecutorTests
{
    public class FakeRequest { }

    [Fact]
    public async Task Execute_ShouldReturnFailure_WhenValidationFails()
    {
        // Arrange
        var request = new FakeRequest();

        var validator = new Mock<IValidator<FakeRequest>>();

        validator
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(
            [
                new ValidationFailure("Field", "Validation error")
            ]));

        var executor = new HandlerExecutor();

        var handlerCalled = false;

        Task<Result<string>> Handler()
        {
            handlerCalled = true;
            return Task.FromResult(Result<string>.Success("ok"));
        }

        // Act
        var result = await executor.Execute(
            request,
            Handler,
            validator.Object,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Validation error");

        handlerCalled.Should().BeFalse();
    }

    [Fact]
    public async Task Execute_ShouldCallHandler_WhenValidationPasses()
    {
        // Arrange
        var request = new FakeRequest();

        var validator = new Mock<IValidator<FakeRequest>>();

        validator
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult()); // válido

        var executor = new HandlerExecutor();

        var handlerCalled = false;

        Task<Result<string>> Handler()
        {
            handlerCalled = true;
            return Task.FromResult(Result<string>.Success("ok"));
        }

        // Act
        var result = await executor.Execute(
            request,
            Handler,
            validator.Object,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("ok");

        handlerCalled.Should().BeTrue();
    }

    [Fact]
    public async Task Execute_ShouldCallHandler_WhenValidatorIsNull()
    {
        // Arrange
        var request = new FakeRequest();
        var executor = new HandlerExecutor();

        var handlerCalled = false;

        Task<Result<string>> Handler()
        {
            handlerCalled = true;
            return Task.FromResult(Result<string>.Success("ok"));
        }

        // Act
        var result = await executor.Execute(
            request,
            Handler,
            null,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        handlerCalled.Should().BeTrue();
    }
}