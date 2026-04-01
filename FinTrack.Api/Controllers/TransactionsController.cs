using FinTrack.Api.Extensions;
using FinTrack.Application.Features.Transactions.Create;
using FinTrack.Application.Features.Transactions.Get;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TransactionsController(
    CreateTransactionHandler createHandler, 
    GetTransactionsHandler getHandler) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateTransactionCommand command,
        CancellationToken cancellationToken)
    {
        var result = await createHandler.Handle(command, cancellationToken);
        return result.ToActionResult(); // quando implementar GetById, passar o id criado para o CreatedAtAction
    }

    [HttpGet]
    public async Task<IActionResult> GetTransactions([FromQuery] GetTransactionsQuery query, CancellationToken cancellationToken)
    {
        var result = await getHandler.Handle(query, cancellationToken);

        return result.ToActionResult();
    }
}
