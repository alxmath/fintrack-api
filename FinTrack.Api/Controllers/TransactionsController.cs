using FinTrack.Application.Transactions.Create;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TransactionsController : ControllerBase
{
    public IActionResult Create(CreateTransactionCommand command)
    {
        var handler = new CreateTransactionHandler();

        var transaction = handler.Handle(command);

        return Ok(transaction);
    }
}
