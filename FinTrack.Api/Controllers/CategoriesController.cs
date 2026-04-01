using FinTrack.Application.Categories.Create;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CategoriesController(CreateCategoryHandler handler) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(command, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        var response = result.Value;
        return CreatedAtAction(nameof(Create), new { id = response.Id }, response);
    }
}
