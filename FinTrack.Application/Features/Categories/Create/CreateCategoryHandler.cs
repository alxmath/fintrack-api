using FinTrack.Application.Common.Errors;
using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Results;
using FinTrack.Domain.Entities;

namespace FinTrack.Application.Features.Categories.Create;

public class CreateCategoryHandler(
    ICategoryRepository repository,
    IUserContext userContext)
    : IRequestHandler<CreateCategoryCommand, CreateCategoryResponse>
{
    public async Task<Result<CreateCategoryResponse>> Handle(
        CreateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var userId = userContext.UserId;

        var exists = await repository
            .ExistsByNameAsync(command.Name, userId, cancellationToken);

        if (exists)
            return Result<CreateCategoryResponse>.Failure(
                new Dictionary<string, string[]>
                {
                    { "Name", ["Categoria já existe"] }
                },
                Errors.General.Conflict);

        var category = new Category(command.Name, userId);

        await repository.AddAsync(category, cancellationToken);

        return Result<CreateCategoryResponse>.Success(
            new CreateCategoryResponse(category.Id, category.Name));
    }
}
