namespace FinTrack.Application.Transactions.Create;

public class CreateTransactionCommand
{
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}
