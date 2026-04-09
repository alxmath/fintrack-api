using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Results;
using FinTrack.Application.Common.Utils;
using FinTrack.Domain.Entities;

namespace FinTrack.Application.Features.Transactions.Create;

public class CreateTransactionHandler(
    ITransactionRepository repository,
    IUserContext userContext)
    : IRequestHandler<CreateTransactionCommand, CreateTransactionResponse>
{
    public async Task<Result<CreateTransactionResponse>> Handle(
        CreateTransactionCommand command,
        CancellationToken cancellationToken)
    {
        var userId = userContext.UserId;

        var date = DateTimeUtils.ToUtc(command.Date);

        var transaction = new Transaction(
            description: command.Description,
            amount: command.Amount,
            date: date,
            categoryId: command.CategoryId,
            userId: userId);

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