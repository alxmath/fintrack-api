using FinTrack.Api.Extensions;
using FinTrack.Application.Common.Dispatching;
using FinTrack.Application.Features.Categories.Create;
using FinTrack.Application.Features.Categories.Get;
using FinTrack.Application.Features.Categories.GetById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class CategoriesController(Dispatcher dispatcher) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var result = await dispatcher.Send(
            command,
            cancellationToken);

        return result.ToActionResult(HttpContext, value =>
            CreatedAtAction(
                nameof(GetById),
                new { id = ((CreateCategoryResponse)value).Id },
                value));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await dispatcher.Send(
            new GetCategoryByIdQuery(id),
            cancellationToken);

        return result.ToActionResult(HttpContext);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await dispatcher.Send(
            new GetCategoriesQuery(),
            cancellationToken);

        return result.ToActionResult(HttpContext);
    }
}
