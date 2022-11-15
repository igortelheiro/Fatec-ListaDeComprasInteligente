using ListaDeComprasInteligente.Domain.Exceptions;
using ListaDeComprasInteligente.Service.Interfaces;
using ListaDeComprasInteligente.Shared.Extensions;
using ListaDeComprasInteligente.Shared.Models.Request;
using ListaDeComprasInteligente.Shared.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace ListaDeComprasInteligente.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ListaComprasController : ControllerBase
{
    private readonly IListaComprasBuilderService _listaComprasBuilderService;

    public ListaComprasController(IListaComprasBuilderService listaComprasBuilderService) =>
        _listaComprasBuilderService = listaComprasBuilderService;
    

    [HttpPost]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<ListaComprasResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Post([FromBody] ListaComprasRequest request)
    {
        try
        {
            var result = await _listaComprasBuilderService.MontarListaComprasAsync(request)
                                                          .LogOnError("Erro ao montar lista de compras");
            return Ok(result);
        }
        catch (DomainException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Ops, houve um problema com a requisição",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Erro ao montar lista de compras",
                    Detail = ex.Message,
                    Extensions = { {"InnerException", ex.InnerException?.Message} },
                    Status = StatusCodes.Status500InternalServerError
                });
        }
    }
}