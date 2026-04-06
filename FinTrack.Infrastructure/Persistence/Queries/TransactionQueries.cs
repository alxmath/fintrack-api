using FinTrack.Application.Features.Transactions.Get;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.Persistence.Queries;

public static class TransactionQueries
{
    public static readonly Func<AppDbContext, Guid, Guid?, DateTime?, DateTime?, int, int, IAsyncEnumerable<GetTransactionsResponse>>
        GetByDateDesc =
        EF.CompileAsyncQuery((
            AppDbContext context,
            Guid userId,
            Guid? categoryId,
            DateTime? startDate,
            DateTime? endDate,
            int skip,
            int take) =>

            context.Transactions
                .AsNoTracking()
                .Where(t =>
                    t.UserId == userId &&
                    (!categoryId.HasValue || t.CategoryId == categoryId) &&
                    (!startDate.HasValue || t.Date >= startDate) &&
                    (!endDate.HasValue || t.Date <= endDate))
                .OrderByDescending(t => t.Date)
                .Skip(skip)
                .Take(take)
                .Select(t => new GetTransactionsResponse(
                    t.Id,
                    t.Amount,
                    t.Description,
                    t.Date,
                    new CategoryDto(
                        t.Category.Id,
                        t.Category.Name
                    )
                ))
        );

    public static readonly Func<AppDbContext, Guid, Guid?, DateTime?, DateTime?, int, int, IAsyncEnumerable<GetTransactionsResponse>>
        GetByDateAsc =
        EF.CompileAsyncQuery((
            AppDbContext context,
            Guid userId,
            Guid? categoryId,
            DateTime? startDate,
            DateTime? endDate,
            int skip,
            int take) =>

            context.Transactions
                .AsNoTracking()
                .Where(t =>
                    t.UserId == userId &&
                    (!categoryId.HasValue || t.CategoryId == categoryId) &&
                    (!startDate.HasValue || t.Date >= startDate) &&
                    (!endDate.HasValue || t.Date <= endDate))
                .OrderBy(t => t.Date)
                .Skip(skip)
                .Take(take)
                .Select(t => new GetTransactionsResponse(
                    t.Id,
                    t.Amount,
                    t.Description,
                    t.Date,
                    new CategoryDto(
                        t.Category.Id,
                        t.Category.Name
                    )
                ))
        );

    public static readonly Func<AppDbContext, Guid, Guid?, DateTime?, DateTime?, int, int, IAsyncEnumerable<GetTransactionsResponse>>
        GetByAmountDesc =
        EF.CompileAsyncQuery((
            AppDbContext context,
            Guid userId,
            Guid? categoryId,
            DateTime? startDate,
            DateTime? endDate,
            int skip,
            int take) =>

            context.Transactions
                .AsNoTracking()
                .Where(t =>
                    t.UserId == userId &&
                    (!categoryId.HasValue || t.CategoryId == categoryId) &&
                    (!startDate.HasValue || t.Date >= startDate) &&
                    (!endDate.HasValue || t.Date <= endDate))
                .OrderByDescending(t => t.Amount)
                .Skip(skip)
                .Take(take)
                .Select(t => new GetTransactionsResponse(
                    t.Id,
                    t.Amount,
                    t.Description,
                    t.Date,
                    new CategoryDto(
                        t.Category.Id,
                        t.Category.Name
                    )
                ))
        );

    public static readonly Func<AppDbContext, Guid, Guid?, DateTime?, DateTime?, int, int, IAsyncEnumerable<GetTransactionsResponse>>
        GetByAmountAsc =
        EF.CompileAsyncQuery((
            AppDbContext context,
            Guid userId,
            Guid? categoryId,
            DateTime? startDate,
            DateTime? endDate,
            int skip,
            int take) =>

            context.Transactions
                .AsNoTracking()
                .Where(t =>
                    t.UserId == userId &&
                    (!categoryId.HasValue || t.CategoryId == categoryId) &&
                    (!startDate.HasValue || t.Date >= startDate) &&
                    (!endDate.HasValue || t.Date <= endDate))
                .OrderByDescending(t => t.Amount)
                .Skip(skip)
                .Take(take)
                .Select(t => new GetTransactionsResponse(
                    t.Id,
                    t.Amount,
                    t.Description,
                    t.Date,
                    new CategoryDto(
                        t.Category.Id,
                        t.Category.Name
                    )
                ))
        );
}
