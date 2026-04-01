using FinTrack.Domain.Entities;

namespace FinTrack.Application.Common.Interfaces;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction, CancellationToken cancellationToken);

    Task<(IReadOnlyList<Transaction> Items, int Total)> SearchAsync(
        int pageNumber, 
        int pageSize,
        Guid? categoryId,
        DateTime? startDate,
        DateTime? endDate,
        string? orderBy,
        bool desc,
        CancellationToken cancellationToken);
}
