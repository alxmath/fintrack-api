namespace FinTrack.Domain.Entities;

public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }

    private Category() { }

    public Category(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome da categoria é obrigatório.", nameof(name));

        if (name.Length > 100)
            throw new ArgumentException("Nome da categoria deve ter no máximo 100 caracteres.", nameof(name));

        Id = Guid.NewGuid();
        Name = name;
        IsActive = true;
    }
}
