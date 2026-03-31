using FluentValidation;

namespace FinTrack.Application.Transactions.Get;

public sealed class GetTransactionsValidator : AbstractValidator<GetTransactionsQuery>
{
    public GetTransactionsValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);
    }
}
