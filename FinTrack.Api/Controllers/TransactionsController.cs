using FinTrack.Api.Extensions;
using FinTrack.Application.Features.Transactions.Create;
using FinTrack.Application.Features.Transactions.Get;
using FinTrack.Application.Features.Transactions.GetById;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TransactionsController(
    CreateTransactionHandler createHandler, 
    GetTransactionsHandler getHandler,
    GetTransactionByIdHandler getByIdHandler) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateTransactionCommand command,
        CancellationToken cancellationToken)
    {
        var result = await createHandler.Handle(command, cancellationToken);
        
        return result.ToActionResult(value =>
            new CreatedAtActionResult(
                actionName: nameof(GetById),
                controllerName: "Transactions",
                routeValues: new { id = value.Id },
                value: result));
    }

    [HttpGet]
    public async Task<IActionResult> GetTransactions(
        [FromQuery] GetTransactionsQuery query, 
        CancellationToken cancellationToken)
    {
        var result = await getHandler.Handle(query, cancellationToken);

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
