using ListaDeComprasInteligente.Domain;
using ListaDeComprasInteligente.Service;
using ListaDeComprasInteligente.Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace ListaDeComprasInteligente.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ListaComprasController : ControllerBase
{
    private readonly ILogger<ListaComprasController> _logger;
    private readonly ListaComprasBuilderService _listaComprasBuilderService;

    public ListaComprasController(ILogger<ListaComprasController> logger, ListaComprasBuilderService listaComprasBuilderService)
    {
        _logger = logger;
        _listaComprasBuilderService = listaComprasBuilderService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(IEnumerable<ListaCompras>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Post([FromBody] ListaComprasRequest request)
    {
        try
        {
            var result = await _listaComprasBuilderService.MontarListaAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro ao montar lista de compras: {Ex}", ex);
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Erro ao montar lista de compras",
                Detail = ex.Message,
                Extensions = { {"InnerException", ex.InnerException?.Message} }
            });
        }
    }
}