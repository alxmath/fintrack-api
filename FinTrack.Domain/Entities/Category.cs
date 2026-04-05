namespace FinTrack.Domain.Entities;

public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }

    public Guid UserId { get; private set; }

    private Category() { }

    public Category(string name, Guid userId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome da categoria é obrigatório.", nameof(name));

        if (name.Length > 100)
            throw new ArgumentException("Nome da categoria deve ter no máximo 100 caracteres.", nameof(name));

        if (userId == Guid.Empty)
            throw new ArgumentException("Usuário inválido.", nameof(userId));

        Id = Guid.NewGuid();
        Name = name;
        UserId = userId;
        IsActive = true;
    }
}
