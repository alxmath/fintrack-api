using FinTrack.Domain.Entities;

namespace FinTrack.Application.Common.Interfaces;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction, CancellationToken cancellationToken);

    Task<IReadOnlyList<Transaction>> GetPagedAsync(
        int pageNumber, 
        int pageSize,
        Guid? categoryId,
        DateTime? startDate,
        DateTime? endDate,
        CancellationToken cancellationToken);
}
