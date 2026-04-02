using FinTrack.Application.Common.Behaviors;
using FinTrack.Application.Common.Errors;
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
        var validation = await ValidationHelper
            .ValidateAsync<CreateCategoryCommand, CreateCategoryResponse>(
                command,
                validator,  
                cancellationToken);

        if (validation.IsFailure)
            return validation;

        var exists = await repository.ExistsByNameAsync(command.Name, cancellationToken);

        if (exists)
            return Result<CreateCategoryResponse>.Failure(
                "Categoria já existe", 
                Errors.General.Conflict);

        var category = new Category(command.Name);

        await repository.AddAsync(category, cancellationToken);

        return Result<CreateCategoryResponse>.Success(new CreateCategoryResponse(category.Id, category.Name));
    }
}
