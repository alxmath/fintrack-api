using FinTrack.Application.Common.Interfaces;
using FinTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.Persistence.Repositories;

public class CategoryRepository(AppDbContext context) : ICategoryRepository
{
    public async Task AddAsync(Category category, CancellationToken cancellationToken)
    {
        context.Categories.Add(category);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string name, Guid userId, CancellationToken cancellationToken)
    {
        return await context.Categories
            .AnyAsync(c => c.Name == name && c.UserId == userId, cancellationToken);
    }

    public async Task<List<Category>> GetAllAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await context.Categories
            .AsNoTracking()
            .Where(c => c.UserId == userId)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Category?> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        return await context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId, cancellationToken);
    }
}
