namespace FinTrack.Application.Features.Transactions.Get;

public class GetTransactionsQuery
{
    public Guid? CategoryId { get; init; }

    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }

    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
     
    public string? OrderBy { get; init; } = "date";
    public bool Desc { get; init; } = true;
}