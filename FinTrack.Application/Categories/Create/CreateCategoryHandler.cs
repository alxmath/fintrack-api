using FinTrack.Application.Common.Results;
using FinTrack.Domain.Entities;
using FluentValidation;

namespace FinTrack.Application.Categories.Create;

public class CreateCategoryHandler(IValidator<CreateCategoryCommand> validator)
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

        var category = new Category(command.Name);

        return Result<CreateCategoryResponse>.Success(new CreateCategoryResponse
        {
            Id = category.Id,
            Nome = category.Name
        });
    }
}
