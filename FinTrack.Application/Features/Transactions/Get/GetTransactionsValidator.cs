using FluentValidation;

namespace FinTrack.Application.Features.Transactions.Get;

public sealed class GetTransactionsValidator : AbstractValidator<GetTransactionsQuery>
{
    public GetTransactionsValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);
    }
}
