using FinTrack.Api.Extensions;
using FinTrack.Application.Features.Categories.Create;
using FinTrack.Application.Features.Categories.Get;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CategoriesController(
    CreateCategoryHandler createHandler,
    GetCategoriesHandler getHandler) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var result = await createHandler.Handle(command, cancellationToken);

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

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await getHandler.Handle(new GetCategoriesQuery(), cancellationToken);

        return result.ToActionResult();
    }
}
