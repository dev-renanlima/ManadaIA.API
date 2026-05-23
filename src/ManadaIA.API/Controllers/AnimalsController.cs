using ManadaIA.Application.DTOs;
using ManadaIA.Application.Features.Animals.Commands;
using ManadaIA.Application.Features.Animals.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManadaIA.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class AnimalsController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Lista todos os animais de uma propriedade
    /// </summary>
    [HttpGet("property/{propertyId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<AnimalDto>), 200)]
    public async Task<IActionResult> GetByProperty(
        Guid propertyId,
        [FromQuery] bool onlyActive = true,
        CancellationToken ct = default)
    {
        var query = new GetAnimalsByPropertyQuery(propertyId, onlyActive);
        var result = await sender.Send(query, ct);
        return Ok(result);
    }

    /// <summary>
    /// Obtém um animal pelo ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AnimalDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct = default)
    {
        var query = new GetAnimalByIdQuery(id);
        var result = await sender.Send(query, ct);
        return Ok(result);
    }

    /// <summary>
    /// Cadastra um novo animal
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(AnimalDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create(
        [FromBody] CreateAnimalRequest request,
        CancellationToken ct = default)
    {
        var command = new CreateAnimalCommand(
            request.EarTag,
            request.Name,
            request.Breed,
            request.Gender,
            request.BirthDate,
            request.PropertyId,
            request.InitialWeight
        );

        var result = await sender.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Remove um animal
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
    {
        // TODO: Implementar DeleteAnimalCommand
        return NoContent();
    }
}
