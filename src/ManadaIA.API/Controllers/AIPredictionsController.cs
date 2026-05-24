using ManadaIA.Application.DTOs;
using ManadaIA.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManadaIA.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/ai")]
[Authorize]
public sealed class AIPredictionsController(IAIPredictionService predictionService) : ControllerBase
{
    /// <summary>
    /// Gera uma predição de prenhez usando IA
    /// </summary>
    [HttpPost("predict")]
    [ProducesResponseType(typeof(AIPredictionDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Predict(
        [FromBody] CreateAIPredictionRequest request,
        CancellationToken ct = default)
    {
        var result = await predictionService.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Obtém uma predição pelo ID
    /// </summary>
    [HttpGet("predictions/{id:guid}")]
    [ProducesResponseType(typeof(AIPredictionDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct = default)
    {
        var result = await predictionService.GetByIdAsync(id, ct);
        return Ok(result);
    }

    /// <summary>
    /// Lista o histórico de predições de um animal
    /// </summary>
    [HttpGet("predictions/animal/{animalId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<AIPredictionDto>), 200)]
    public async Task<IActionResult> GetByAnimalId(Guid animalId, CancellationToken ct = default)
    {
        var result = await predictionService.GetByAnimalIdAsync(animalId, ct);
        return Ok(result);
    }
}
