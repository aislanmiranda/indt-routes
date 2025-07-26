using Application.Interfaces;
using Application.Requests;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class routeController : ControllerBase
{
    private readonly IRouteService _routeService;

    public routeController(IRouteService routeService)
        => _routeService = routeService;

    [HttpGet("searchBestRoutes")]
    [SwaggerOperation(Summary = "Buscar melhor rota", Description = "Busca a melhor rota com base no request de origem e destino.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Rota encontrada com sucesso", typeof(object))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Request inválido")]
    public async Task<IActionResult> GetSearchBestRoute([FromQuery] SearchFlightsRequest request, CancellationToken cancellationToken)
    {
        var result = await _routeService.GetSearchBestRoute(request, cancellationToken);

        if (!result.Success)
            return StatusCode(result.StatusCode, new { error = result.Error });

        return StatusCode(result.StatusCode, new { data = result.Data });
    }

    [HttpPost("create")]
    [SwaggerOperation(
        Summary = "Criar rota",
        Description = "Cria uma nova rota com as informações fornecidas.")]
    [SwaggerResponse(StatusCodes.Status201Created, "Rota criada com sucesso", typeof(object))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos")]
    public async Task<IActionResult> CreateRoute(CreateRouteRequest request, CancellationToken cancellationToken)
    {
        var result = await _routeService.AddRouteAsync(request, cancellationToken);

        if (!result.Success)
            return StatusCode(result.StatusCode, new { error = result.Error });

        return StatusCode(result.StatusCode, new { data = result.Data });
    }

    [HttpPut("update")]
    [SwaggerOperation(
        Summary = "Atualizar rota",
        Description = "Atualiza os dados de uma rota existente.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Rota atualizada com sucesso", typeof(object))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos")]
    public async Task<IActionResult> UpdateRoute(UpdateRouteRequest request, CancellationToken cancellationToken)
    {
        var result = await _routeService.UpdateRouteAsync(request, cancellationToken);

        if (!result.Success)
            return StatusCode(result.StatusCode, new { error = result.Error });

        return StatusCode(result.StatusCode, new { data = result.Data });
    }

    [HttpDelete("delete")]
    [SwaggerOperation(
        Summary = "Deletar rota",
        Description = "Remove uma rota pelo ID informado.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Rota removida com sucesso", typeof(object))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Rota não encontrada")]
    public async Task<IActionResult> DeleteRoute([FromQuery] int id, CancellationToken cancellationToken)
    {
        var result = await _routeService.DeleteRouteAsync(id, cancellationToken);

        if (!result.Success)
            return StatusCode(result.StatusCode, new { error = result.Error });

        return StatusCode(result.StatusCode, new { data = result.Data });
    }

    [HttpGet("all")]
    [SwaggerOperation(
        Summary = "Listar todas as rotas",
        Description = "Retorna uma lista com todas as rotas cadastradas.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Lista de rotas", typeof(object))]
    public async Task<IActionResult> GetAllRoutes(CancellationToken cancellationToken)
    {
        var result = await _routeService.GetAllRoutesAsync(cancellationToken);

        if (!result.Success)
            return StatusCode(result.StatusCode, new { error = result.Error });

        return StatusCode(result.StatusCode, new { data = result.Data });
    }
}

