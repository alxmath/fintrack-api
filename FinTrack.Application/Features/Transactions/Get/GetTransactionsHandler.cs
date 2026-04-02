using FinTrack.Application.Common.Behaviors;
using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Results;
using FluentValidation;

namespace FinTrack.Application.Features.Transactions.Get;

public sealed class GetTransactionsHandler(ITransactionRepository repository,
     IValidator<GetTransactionsQuery> validator)
{
    public async Task<Result<PagedResult<GetTransactionsResponse>>> Handle(
        GetTransactionsQuery query, 
        CancellationToken cancellationToken)
    {
        var validation = await ValidationHelper
            .ValidateAsync<GetTransactionsQuery, PagedResult<GetTransactionsResponse>>(
                query,
                validator,
                cancellationToken);

        if (validation.IsFailure)
            return validation;

        var (transactions, total) = await repository.SearchAsync(
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
                t.Date))
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
