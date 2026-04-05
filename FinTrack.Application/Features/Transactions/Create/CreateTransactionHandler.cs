using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Results;
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

        var transaction = new Transaction(
            command.Description,
            command.Amount,
            command.Date,
            command.CategoryId,
            userId);

        // TODO: salvar userId na entidade

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