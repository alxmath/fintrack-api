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
        string? orderBy,
        bool desc,
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

        query = orderBy?.ToLower() switch
        {
            "amount" => desc
                ? query.OrderByDescending(t => t.Amount)
                : query.OrderBy(t => t.Amount),

            _ => desc
                ? query.OrderByDescending(t => t.Date)
                : query.OrderBy(t => t.Date),
        };

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, total);
    }

    public async Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Transactions
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }
}
