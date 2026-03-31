namespace FinTrack.Application.Transactions.Get;

public sealed record GetTransactionsQuery(
    int PageNumber = 1, 
    int PageSize = 10);