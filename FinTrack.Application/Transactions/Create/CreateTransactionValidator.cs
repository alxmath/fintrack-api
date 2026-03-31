using FluentValidation;

namespace FinTrack.Application.Transactions.Create;

public class CreateTransactionValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Descrição é obrigatória")
            .MaximumLength(200);

        RuleFor(x => x.Amount)
            .NotEqual(0).WithMessage("Valor não pode ser zero");

        RuleFor(x => x.Date)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Data não pode ser futura");
    }
}
