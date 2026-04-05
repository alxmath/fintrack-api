using FinTrack.Application.Common.Errors;
using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Results;

namespace FinTrack.Application.Features.Categories.GetById;

public class GetCategoryByIdHandler(
    ICategoryRepository repository,
    IUserContext userContext)
    : IRequestHandler<GetCategoryByIdQuery, GetCategoryByIdResponse>
{
    public async Task<Result<GetCategoryByIdResponse>> Handle(
        GetCategoryByIdQuery request, 
        CancellationToken cancellationToken)
    {
        var userId = userContext.UserId;
        var category = await repository.GetByIdAsync(request.Id, userId, cancellationToken);

        if (category is null)
            return Result<GetCategoryByIdResponse>.Failure(
                new Dictionary<string, string[]>
                {
                    { "Name", ["Categoria não encontrada"] }
                },
                Errors.General.NotFound);

        var response = new GetCategoryByIdResponse(category.Id, category.Name);

        return Result<GetCategoryByIdResponse>.Success(response);
    }
}
