using FinTrack.Api.Extensions;
using FinTrack.Application.Common.Dispatching;
using FinTrack.Application.Features.Transactions.Create;
using FinTrack.Application.Features.Transactions.Get;
using FinTrack.Application.Features.Transactions.GetById;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TransactionsController(Dispatcher dispatcher) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateTransactionCommand command,
        CancellationToken cancellationToken)
    {
        var result = await dispatcher.Send(
            command,
            cancellationToken);

        return result.ToActionResult(HttpContext, value =>
             CreatedAtAction(
                 nameof(GetById),
                 new { id = ((CreateTransactionResponse)value).Id },
                 value));
    }

    [HttpGet]
    public async Task<IActionResult> GetTransactions(
        [FromQuery] GetTransactionsQuery query,
        CancellationToken cancellationToken)
    {
        var result = await dispatcher.Send(
            query,
            cancellationToken);

        return result.ToActionResult(HttpContext);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetTransactionByIdQuery(id);

        var result = await dispatcher.Send(
            query,
            cancellationToken);

        return result.ToActionResult(HttpContext);
    }
}
