namespace FinTrack.Application.Features.Transactions.Get;

public sealed record GetTransactionsResponse(
    Guid Id,
    decimal Amount,
    string Description,
    DateTime Date);
