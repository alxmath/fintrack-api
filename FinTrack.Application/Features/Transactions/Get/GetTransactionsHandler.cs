using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Results;

namespace FinTrack.Application.Features.Transactions.Get;

public sealed class GetTransactionsHandler(ITransactionRepository repository,
    IUserContext userContext)
    : IRequestHandler<GetTransactionsQuery, PagedResult<GetTransactionsResponse>>
{
    public async Task<Result<PagedResult<GetTransactionsResponse>>> Handle(
        GetTransactionsQuery query, 
        CancellationToken cancellationToken)
    {
        var userId = userContext.UserId;

        var (transactions, total) = await repository.SearchAsync(
            userId,
            query.Page,
            query.PageSize,
            query.CategoryId,
            query.StartDate,
            query.EndDate,
            query.OrderBy,
            query.Desc,
            cancellationToken);

        var items = transactions
            .Select(t => new GetTransactionsResponse(
                t.Id,
                t.Amount,
                t.Description,
                t.Date,
                new CategoryDto(
                    t.Category.Id,
                    t.Category.Name
                )))
            .ToList();

        var result = new PagedResult<GetTransactionsResponse>
        {
            Items = items,
            Total = total,
            Page = query.Page,
            PageSize = query.PageSize
        };

        return Result<PagedResult<GetTransactionsResponse>>.Success(result);
    }
}
