using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Results;
using FinTrack.Domain.Entities;
using FluentValidation;

namespace FinTrack.Application.Features.Categories.Create;

public class CreateCategoryHandler(
    ICategoryRepository repository,
    IValidator<CreateCategoryCommand> validator)
{
    public async Task<Result<CreateCategoryResponse>> Handle(
        CreateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var error = validationResult.Errors.First().ErrorMessage;
            return Result<CreateCategoryResponse>.Failure(error);
        }

        var exists = await repository.ExistsByNameAsync(command.Name, cancellationToken);

        if (exists)
            return Result<CreateCategoryResponse>.Failure("Categoria já existe");

        var category = new Category(command.Name);

        await repository.AddAsync(category, cancellationToken);

        return Result<CreateCategoryResponse>.Success(new CreateCategoryResponse(category.Id, category.Name));
    }
}
