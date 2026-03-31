using FinTrack.Api.Extensions;
using FinTrack.Application.Transactions.Create;
using FinTrack.Application.Transactions.Get;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TransactionsController(
    CreateTransactionHandler createHandler, 
    GetTransactionsHandler getHandler) : ControllerBase
{
    public async Task<IActionResult> Create(
        CreateTransactionCommand command,
        CancellationToken cancellationToken)
    {
        var result = await createHandler.Handle(command, cancellationToken);
        return result.ToActionResult();
    }

    public async Task<IActionResult> GetTransactions(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetTransactionsQuery(pageNumber, pageSize);
        
        var result = await getHandler.Handle(query, cancellationToken);

        return Ok(result);
    }
}
