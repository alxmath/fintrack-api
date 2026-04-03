using FinTrack.Api.Extensions;
using FinTrack.Application.Common.Dispatching;
using FinTrack.Application.Features.Categories.Create;
using FinTrack.Application.Features.Categories.Get;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CategoriesController(Dispatcher dispatcher) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var result = await dispatcher.Send<CreateCategoryCommand, CreateCategoryResponse>(
            command,
            cancellationToken);

        return result.ToActionResult();
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
        var result = await dispatcher.Send<GetCategoriesQuery, List<GetCategoriesResponse>>(
            new GetCategoriesQuery(),
            cancellationToken);

        return result.ToActionResult();
    }
}
