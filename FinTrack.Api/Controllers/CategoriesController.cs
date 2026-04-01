using FinTrack.Api.Extensions;
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

        return result.ToActionResult(value =>
            new CreatedAtActionResult(
                actionName: nameof(GetById),
                controllerName: "Categories",
                routeValues: new { id = value.Id },
                value: result));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        // placeholder até implementar
        return Ok();
    }
}
