using FinTrack.Application.Common.Interfaces;
using FinTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.Persistence.Repositories;

public class TransactionRepository(AppDbContext context) : ITransactionRepository
{
    public async Task AddAsync(Transaction transaction, CancellationToken cancellationToken)
    {
        await context.Transactions.AddAsync(transaction, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<(IReadOnlyList<Transaction> Items, int Total)> SearchAsync(
        int pageNumber,
        int pageSize,
        Guid? categoryId,
        DateTime? startDate,
        DateTime? endDate,
        CancellationToken cancellationToken)
    {
        var query = context.Transactions
            .AsNoTracking();

        if (categoryId.HasValue)
            query = query.Where(t => t.CategoryId == categoryId);

        if (startDate.HasValue)
            query = query.Where(t => t.Date >= startDate);

        if (endDate.HasValue)
            query = query.Where(t => t.Date <= endDate);

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(t => t.Date)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, total);
    }
}
