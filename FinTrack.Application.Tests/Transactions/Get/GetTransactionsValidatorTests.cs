using FinTrack.Application.Features.Transactions.Get;
using FluentAssertions;

namespace FinTrack.Application.Tests.Transactions.Get;

public class GetTransactionsValidatorTests
{
    [Fact]
    public void Validate_ShouldPass_WhenQueryIsValid()
    {
        // Arrange
        var validator = new GetTransactionsValidator();

        var query = new GetTransactionsQuery(1, 10);

        // Act
        var result = validator.Validate(query);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_ShouldFail_WhenPageNumberIsZero()
    {
        // Arrange
        var validator = new GetTransactionsValidator();

        var query = new GetTransactionsQuery(0, 10);

        // Act
        var result = validator.Validate(query);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "PageNumber");
    }

    [Fact]
    public void Validate_ShouldFail_WhenPageSizeIsZero()
    {
        var validator = new GetTransactionsValidator();

        var query = new GetTransactionsQuery(1, 0);

        var result = validator.Validate(query);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "PageSize");
    }

    [Fact]
    public void Validate_ShouldFail_WhenPageSizeIsTooLarge()
    {
        var validator = new GetTransactionsValidator();

        var query = new GetTransactionsQuery(1, 200);

        var result = validator.Validate(query);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "PageSize");
    }
}
