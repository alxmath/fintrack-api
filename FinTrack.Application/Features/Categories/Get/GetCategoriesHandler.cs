using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Results;

namespace FinTrack.Application.Features.Categories.Get;

public class GetCategoriesHandler(ICategoryRepository repository)
{
    public async Task<Result<List<CategoryResponse>>> Handle(
        GetCategoriesQuery query,
        CancellationToken cancellationToken)
    {
        var categories = await repository.GetAllAsync(cancellationToken);

        var response = categories
            .Select(c => new CategoryResponse(c.Id, c.Name))
            .ToList();

        return Result<List<CategoryResponse>>.Success(response);
    }
}
