using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Results;

namespace FinTrack.Application.Features.Categories.Get;

public class GetCategoriesHandler(ICategoryRepository repository)
    : IRequestHandler<GetCategoriesQuery, List<GetCategoriesResponse>>
{
    public async Task<Result<List<GetCategoriesResponse>>> Handle(
        GetCategoriesQuery query,
        CancellationToken cancellationToken)
    {
        var categories = await repository.GetAllAsync(cancellationToken);

        var response = categories
            .Select(c => new GetCategoriesResponse(c.Id, c.Name))
            .ToList();

        return Result<List<GetCategoriesResponse>>.Success(response);
    }
}
