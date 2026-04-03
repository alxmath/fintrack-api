using FinTrack.Api.Extensions;
using FinTrack.Application.Common.Execution;
using FinTrack.Application.Features.Transactions.Create;
using FinTrack.Application.Features.Transactions.Get;
using FinTrack.Application.Features.Transactions.GetById;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TransactionsController(
    CreateTransactionHandler createHandler, 
    GetTransactionsHandler getHandler,
    GetTransactionByIdHandler getByIdHandler,
    IValidator<CreateTransactionCommand> createValidator,
    IValidator<GetTransactionsQuery> getValidator,
    HandlerExecutor executor) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateTransactionCommand command,
        CancellationToken cancellationToken)
    {
        var result = await executor.Execute(
            command,
            () => createHandler.Handle(command, cancellationToken),
            createValidator,
            cancellationToken);

        return result.ToActionResult();
    }

    [HttpGet]
    public async Task<IActionResult> GetTransactions(
        [FromQuery] GetTransactionsQuery query, 
        CancellationToken cancellationToken)
    {
        var result = await executor.Execute(
            query,
            () => getHandler.Handle(query, cancellationToken),
            getValidator,
            cancellationToken);

        return result.ToActionResult();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id, 
        CancellationToken cancellationToken)
    {
        var query = new GetTransactionByIdQuery(id);

        var result = await getByIdHandler.Handle(query, cancellationToken);

        return result.ToActionResult();
    }
}
