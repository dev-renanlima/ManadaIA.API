using ManadaIA.Application.DTOs;
using ManadaIA.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ManadaIA.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/reports")]
[Authorize]
public sealed class ReportsController(IReportService reportService) : ControllerBase
{
    /// <summary>
    /// Obtém um resumo geral dos animais do usuário
    /// </summary>
    [HttpGet("summary")]
    [ProducesResponseType(typeof(SummaryReportDto), 200)]
    public async Task<IActionResult> GetSummary(CancellationToken ct = default)
    {
        var userId = GetUserId();
        var result = await reportService.GetSummaryAsync(userId, ct);
        return Ok(result);
    }

    /// <summary>
    /// Obtém taxa de prenhez por espécie
    /// </summary>
    [HttpGet("pregnancy-rate")]
    [ProducesResponseType(typeof(IReadOnlyList<PregnancyRateReportDto>), 200)]
    public async Task<IActionResult> GetPregnancyRate(CancellationToken ct = default)
    {
        var userId = GetUserId();
        var result = await reportService.GetPregnancyRateAsync(userId, ct);
        return Ok(result);
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                          ?? User.FindFirst("sub")?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            throw new UnauthorizedAccessException("Usuário não autenticado");

        return userId;
    }
}
