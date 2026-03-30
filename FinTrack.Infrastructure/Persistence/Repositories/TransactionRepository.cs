using FinTrack.Application.Common.Interfaces;
using FinTrack.Domain.Entities;

namespace FinTrack.Infrastructure.Persistence.Repositories;

public class TransactionRepository(AppDbContext context) : ITransactionRepository
{
    public async Task AddAsync(Transaction transaction, CancellationToken cancellationToken)
    {
        await context.Transactions.AddAsync(transaction, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}
