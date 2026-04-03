using FinTrack.Domain.Entities;

namespace FinTrack.Application.Common.Interfaces;

public interface ICategoryRepository
{
    Task AddAsync(Category category, CancellationToken cancellationToken);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken);
    Task<List<Category>> GetAllAsync(CancellationToken cancellationToken);
    Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
