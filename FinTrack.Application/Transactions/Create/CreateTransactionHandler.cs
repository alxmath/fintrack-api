using FinTrack.Application.Common.Interfaces;
using FinTrack.Domain.Entities;

namespace FinTrack.Application.Transactions.Create;

public class CreateTransactionHandler(ITransactionRepository repository)
{
    public async Task<Transaction> Handle(CreateTransactionCommand command, CancellationToken cancellationToken)
    {
        var transaction = new Transaction(
            command.Description,
            command.Amount,
            command.Date);

        await repository.AddAsync(transaction, cancellationToken);

        return transaction;
    }
}