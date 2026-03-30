using FinTrack.Domain.Entities;

namespace FinTrack.Application.Common.Interfaces;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction, CancellationToken cancellationToken);
}
