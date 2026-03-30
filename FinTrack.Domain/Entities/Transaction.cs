namespace FinTrack.Domain.Entities;

public class Transaction
{
    public Guid Id { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public DateTime Date { get; private set; }

    public Transaction() { }

    public Transaction(string description, decimal amount, DateTime date)
    {
        Id = Guid.NewGuid();
        Description = description;
        Amount = amount;
        Date = date;
    }
}
