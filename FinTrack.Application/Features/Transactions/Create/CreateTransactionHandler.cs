using FinTrack.Application.Common.Behaviors;
using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Results;
using FinTrack.Domain.Entities;
using FluentValidation;

namespace FinTrack.Application.Features.Transactions.Create;

public class CreateTransactionHandler(
    ITransactionRepository repository,
    IValidator<CreateTransactionCommand> validator)
{
    public async Task<Result<CreateTransactionResponse>> Handle(
        CreateTransactionCommand command, 
        CancellationToken cancellationToken)
    {
        var validation = await ValidationHelper
            .ValidateAsync(
                command,
                validator,
                cancellationToken);

        if (validation is not null)
            return Result<CreateTransactionResponse>.Failure(
                validation.Error,
                validation.ErrorCode);

        var transaction = new Transaction(
            command.Description,
            command.Amount,
            command.Date,
            command.CategoryId);

        await repository.AddAsync(transaction, cancellationToken);

        var response = new CreateTransactionResponse
        {
            Id = transaction.Id,
            Description = transaction.Description,
            Amount = transaction.Amount,
            Date = transaction.Date
        };

        return Result<CreateTransactionResponse>.Success(response);
    }
}