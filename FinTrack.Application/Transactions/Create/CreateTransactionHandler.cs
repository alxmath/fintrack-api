using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Results;
using FinTrack.Domain.Entities;

namespace FinTrack.Application.Transactions.Create;

public class CreateTransactionHandler(ITransactionRepository repository)
{
    public async Task<Result<Transaction>> Handle(CreateTransactionCommand command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.Description))
            return Result<Transaction>.Failure("Descrição é obrigatória");

        if (command.Amount == 0)
            return Result<Transaction>.Failure("Valor não pode ser zero");

        var transaction = new Transaction(
            command.Description,
            command.Amount,
            command.Date);

        await repository.AddAsync(transaction, cancellationToken);

        return Result<Transaction>.Success(transaction);
    }
}