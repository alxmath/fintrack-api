using FluentValidation;

namespace FinTrack.Application.Features.Categories.Get;

public class GetCategoriesValidator : AbstractValidator<GetCategoriesQuery>
{
    public GetCategoriesValidator()
    {
        // sem regras por enquanto
    }
}
