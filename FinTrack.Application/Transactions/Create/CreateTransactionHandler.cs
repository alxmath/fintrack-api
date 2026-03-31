using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Results;
using FinTrack.Domain.Entities;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace FinTrack.Application.Transactions.Create;

public class CreateTransactionHandler(
    ITransactionRepository repository,
    IValidator<CreateTransactionCommand> validator)
{
    public async Task<Result<Transaction>> Handle(CreateTransactionCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var error = validationResult.Errors.First().ErrorMessage;
            return Result<Transaction>.Failure(error);
        }

        var transaction = new Transaction(
            command.Description,
            command.Amount,
            command.Date);

        await repository.AddAsync(transaction, cancellationToken);

        return Result<Transaction>.Success(transaction);
    }
}