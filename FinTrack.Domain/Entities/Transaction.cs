namespace FinTrack.Domain.Entities;

public class Transaction
{
    public Guid Id { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public DateTime Date { get; private set; }
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; } = default!;
    public Guid UserId { get; private set; }

    private Transaction() {}

    public static Transaction Create(
        string description,
        decimal amount,
        DateTime date,
        Guid categoryId,
        Guid userId,
        DateTime now)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Descrição é obrigatória.", nameof(description));

        if (description.Length > 200)
            throw new ArgumentException("Descrição deve ter no máximo 200 caracteres.", nameof(description));

        if (amount == 0)
            throw new ArgumentException("Valor não pode ser zero.", nameof(amount));

        if (date > now)
            throw new ArgumentException("Data não pode ser futura.");

        if (categoryId == Guid.Empty)
            throw new ArgumentException("Categoria inválida.", nameof(categoryId));

        if (userId == Guid.Empty)
            throw new ArgumentException("Usuário inválido.", nameof(userId));

        return new Transaction
        {
            Id = Guid.NewGuid(),
            Description = description.Trim(),
            Amount = amount,
            Date = date,
            CategoryId = categoryId,
            UserId = userId,
        };
    }
}
