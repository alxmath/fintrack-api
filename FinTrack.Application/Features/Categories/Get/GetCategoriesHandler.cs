using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Results;

namespace FinTrack.Application.Features.Categories.Get;

public class GetCategoriesHandler(
    ICategoryRepository repository,
    IUserContext userContext)
    : IRequestHandler<GetCategoriesQuery, List<GetCategoriesResponse>>
{
    public async Task<Result<List<GetCategoriesResponse>>> Handle(
        GetCategoriesQuery query,
        CancellationToken cancellationToken)
    {
        var userId = userContext.UserId;
        var categories = await repository.GetAllAsync(userId, cancellationToken);

        var response = categories
            .Select(c => new GetCategoriesResponse(c.Id, c.Name))
            .ToList();

        return Result<List<GetCategoriesResponse>>.Success(response);
    }
}
