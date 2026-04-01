namespace FinTrack.Domain.Entities;

public class Transaction
{
    public Guid Id { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public DateTime Date { get; private set; }
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; } = default!;

    public Transaction() { }

    public Transaction(string description, decimal amount, DateTime date, Guid categoryId)
    {
        Id = Guid.NewGuid();
        Description = description;
        Amount = amount;
        Date = date;
        CategoryId = categoryId;
    }
}
