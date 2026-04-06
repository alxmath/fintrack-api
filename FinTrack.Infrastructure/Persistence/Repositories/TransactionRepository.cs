using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Features.Transactions.Get;
using FinTrack.Domain.Entities;
using FinTrack.Infrastructure.Persistence.Queries;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.Persistence.Repositories;

public class TransactionRepository(AppDbContext context) : ITransactionRepository
{
    public async Task AddAsync(Transaction transaction, CancellationToken cancellationToken)
    {
        await context.Transactions.AddAsync(transaction, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<(IReadOnlyList<GetTransactionsResponse> Items, int Total)> SearchAsync(
        Guid userId,
        int pageNumber,
        int pageSize,
        Guid? categoryId,
        DateTime? startDate,
        DateTime? endDate,
        string? orderBy,
        bool desc,
        CancellationToken cancellationToken)
    {
        var compiledQuery = (orderBy?.ToLower(), desc) switch
        {
            ("amount", true) => TransactionQueries.GetByAmountDesc,
            ("amount", false) => TransactionQueries.GetByAmountAsc,
            ("date", true) => TransactionQueries.GetByDateDesc,
            ("date", false) => TransactionQueries.GetByDateAsc,
            _ => TransactionQueries.GetByDateDesc
        };

        var skip = (pageNumber - 1) * pageSize;

        var items = await compiledQuery(
                context,
                userId,
                categoryId,
                startDate,
                endDate,
                skip,
                pageSize)
            .ToListAsync(cancellationToken);

        var total = await context.Transactions
            .Where(t =>
                t.UserId == userId &&
                (!categoryId.HasValue || t.CategoryId == categoryId) &&
                (!startDate.HasValue || t.Date >= startDate) &&
                (!endDate.HasValue || t.Date <= endDate))
            .CountAsync(cancellationToken);

        return (items, total);
    }

    public async Task<Transaction?> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        return await context.Transactions
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId, cancellationToken);
    }
}
