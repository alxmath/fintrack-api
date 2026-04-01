namespace FinTrack.Application.Categories.Create;

public class CreateCategoryResponse
{
    public Guid Id { get; init; }
    public string Nome { get; set; } = string.Empty;
}
