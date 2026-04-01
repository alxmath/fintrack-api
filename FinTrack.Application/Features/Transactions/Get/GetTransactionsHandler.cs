using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Results;
using FluentValidation;

namespace FinTrack.Application.Features.Transactions.Get;

public sealed class GetTransactionsHandler(ITransactionRepository repository,
     IValidator<GetTransactionsQuery> validator)
{
    public async Task<Result<IReadOnlyList<GetTransactionsResponse>>> Handle(
        GetTransactionsQuery query, 
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(query, cancellationToken);

        if (!validationResult.IsValid)
        {
            var error = validationResult.Errors.First().ErrorMessage;
            return Result<IReadOnlyList<GetTransactionsResponse>>.Failure(error);
        }

        var transactions = await repository.GetPagedAsync(
            query.PageNumber,
            query.PageSize,
            cancellationToken);

        var response = transactions
            .Select(t => new GetTransactionsResponse(
                t.Id,
                t.Amount,
                t.Description,
                t.Date))
            .ToList();

        return Result<IReadOnlyList<GetTransactionsResponse>>.Success(response);
    }
}
