namespace FinTrack.Application.Transactions.Get;

public sealed record GetTransactionsResponse(
    Guid Id,
    decimal Amount,
    string Description,
    DateTime Date);
