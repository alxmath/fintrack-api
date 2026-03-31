using FinTrack.Application.Common.Interfaces;

namespace FinTrack.Application.Transactions.Get;

public sealed class GetTransactionsHandler(ITransactionRepository repository)
{
    public async Task<IReadOnlyList<GetTransactionsResponse>> Handle(
        GetTransactionsQuery query, 
        CancellationToken cancellationToken)
    {
        var transactions = await repository.GetPagedAsync(
            query.PageNumber,
            query.PageSize,
            cancellationToken);

        return transactions
            .OrderByDescending(t => t.Date)
            .Select(t => new GetTransactionsResponse(
                t.Id,
                t.Amount,
                t.Description,
                t.Date))
            .ToList();
    }
}
