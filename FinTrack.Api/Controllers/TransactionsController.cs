using FinTrack.Api.Extensions;
using FinTrack.Application.Transactions.Create;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TransactionsController(CreateTransactionHandler handler) : ControllerBase
{
    public async Task<IActionResult> Create(
        CreateTransactionCommand command,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(command, cancellationToken);
        return result.ToActionResult();
    }
}
