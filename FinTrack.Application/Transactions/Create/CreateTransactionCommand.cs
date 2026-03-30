using FinTrack.Domain.Entities;

namespace FinTrack.Application.Transactions.Create;

public class CreateTransactionCommand
{
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}

public class CreateTransactionHandler
{
    public Transaction Handle(CreateTransactionCommand command)
    {
        return new Transaction(
            command.Description,
            command.Amount,
            command.Date);
    }
}