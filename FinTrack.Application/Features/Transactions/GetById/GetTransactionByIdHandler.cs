using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Results;

namespace FinTrack.Application.Features.Transactions.GetById;

public class GetTransactionByIdHandler(ITransactionRepository repository)
{
    public async Task<Result<GetTransactionByIdResponse>> Handle(
        GetTransactionByIdQuery query,
        CancellationToken cancellationToken)
    {
        var transaction = await repository.GetByIdAsync(query.Id, cancellationToken);
        
        if (transaction is null)
            return Result<GetTransactionByIdResponse>.Failure("Transação não encontrada");

        var response = new GetTransactionByIdResponse(
            transaction.Id,
            transaction.Amount,
            transaction.Description,
            transaction.Date
        );

        return Result<GetTransactionByIdResponse>.Success(response);
    }
}
