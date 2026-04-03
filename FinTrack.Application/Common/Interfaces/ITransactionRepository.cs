using FinTrack.Application.Features.Transactions.Get;
using FinTrack.Domain.Entities;

namespace FinTrack.Application.Common.Interfaces;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction, CancellationToken cancellationToken);

    Task<(IReadOnlyList<GetTransactionsResponse> Items, int Total)> SearchAsync(
        int pageNumber, 
        int pageSize,
        Guid? categoryId,
        DateTime? startDate,
        DateTime? endDate,
        string? orderBy,
        bool desc,
        CancellationToken cancellationToken);

    Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
