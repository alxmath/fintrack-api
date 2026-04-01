namespace FinTrack.Domain.Entities;

public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }

    private Category() { }

    public Category(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
        IsActive = true;
    }
}
