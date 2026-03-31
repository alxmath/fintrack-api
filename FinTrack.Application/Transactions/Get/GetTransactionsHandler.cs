using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Results;

namespace FinTrack.Application.Transactions.Get;

public sealed class GetTransactionsHandler(ITransactionRepository repository)
{
    public async Task<Result<IReadOnlyList<GetTransactionsResponse>>> Handle(
        GetTransactionsQuery query, 
        CancellationToken cancellationToken)
    {
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
