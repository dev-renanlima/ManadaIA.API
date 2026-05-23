using ManadaIA.Application.DTOs;
using ManadaIA.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ManadaIA.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/cycles")]
[Authorize]
public sealed class ReproductiveCyclesController(IReproductiveCycleService cycleService) : ControllerBase
{
    /// <summary>
    /// Obtém o histórico de ciclos reprodutivos de um animal
    /// </summary>
    [HttpGet("animals/{animalId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<ReproductiveCycleDto>), 200)]
    public async Task<IActionResult> GetByAnimalId(Guid animalId, CancellationToken ct = default)
    {
        var result = await cycleService.GetByAnimalIdAsync(animalId, ct);
        return Ok(result);
    }

    /// <summary>
    /// Registra um novo evento reprodutivo (inseminação, parto, diagnóstico)
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ReproductiveCycleDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create(
        [FromBody] CreateReproductiveCycleRequest request,
        CancellationToken ct = default)
    {
        var result = await cycleService.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Obtém um ciclo reprodutivo pelo ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ReproductiveCycleDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct = default)
    {
        var result = await cycleService.GetByIdAsync(id, ct);
        return Ok(result);
    }

    /// <summary>
    /// Atualiza o resultado de um ciclo (prenha/vazia)
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ReproductiveCycleDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateResult(
        Guid id,
        [FromBody] UpdateReproductiveCycleRequest request,
        CancellationToken ct = default)
    {
        var result = await cycleService.UpdateResultAsync(id, request, ct);
        return Ok(result);
    }
}
