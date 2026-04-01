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
        var validationResult = await validator.ValidateAsync(query, cancellationToken);

        if (!validationResult.IsValid)
        {
            var error = validationResult.Errors.First().ErrorMessage;
            return Result<PagedResult<GetTransactionsResponse>>.Failure(error);
        }

        var (transactions, total) = await repository.SearchAsync(
            query.Page,
            query.PageSize,
            query.CategoryId,
            query.StartDate,
            query.EndDate,
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
