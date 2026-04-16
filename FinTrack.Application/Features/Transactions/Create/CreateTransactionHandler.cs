using FinTrack.Application.Common.Abstractions;
using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Results;
using FinTrack.Application.Common.Utils;
using FinTrack.Domain.Entities;
using static FinTrack.Application.Common.Errors.Errors;

namespace FinTrack.Application.Features.Transactions.Create;

public class CreateTransactionHandler(
    ITransactionRepository transactionRepository,
    ICategoryRepository categoryRepository,
    IUserContext userContext,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<CreateTransactionCommand, CreateTransactionResponse>
{
    public async Task<Result<CreateTransactionResponse>> Handle(
        CreateTransactionCommand command,
        CancellationToken cancellationToken)
    {
        var userId = userContext.UserId;

        var date = DateTimeUtils.ToUtc(command.Date);

        var category = await categoryRepository
            .GetByIdAsync(command.CategoryId, userId, cancellationToken);

        if (category is null)
            return Result<CreateTransactionResponse>.Failure(
                new Dictionary<string, string[]>
                {
                    { nameof(command.CategoryId), ["Categoria não encontrada"] }
                },
                General.NotFound);

        var now = dateTimeProvider.UtcNow;

        var transaction = Transaction.Create(
            description: command.Description,
            amount: command.Amount,
            date: date,
            categoryId: category.Id,
            userId: userId,
            now: now);

        await transactionRepository.AddAsync(transaction, cancellationToken);

        var response = new CreateTransactionResponse
        {
            Id = transaction.Id,
            Description = transaction.Description,
            Amount = transaction.Amount,
            Date = transaction.Date
        };

        return Result<CreateTransactionResponse>.Success(response);
    }
}