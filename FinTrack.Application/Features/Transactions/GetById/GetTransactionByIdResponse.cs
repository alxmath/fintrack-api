namespace FinTrack.Application.Features.Transactions.GetById;

public sealed record GetTransactionByIdResponse(
    Guid Id,
    decimal Amount,
    string Description,
    DateTime Date
);
