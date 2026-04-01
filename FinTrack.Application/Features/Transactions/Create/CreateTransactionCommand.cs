namespace FinTrack.Application.Features.Transactions.Create;

public sealed record CreateTransactionCommand(string Description, decimal Amount, DateTime Date, Guid CategoryId);