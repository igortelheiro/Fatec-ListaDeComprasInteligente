using ListaDeComprasInteligente.Shared.Models.Request;
using ListaDeComprasInteligente.Shared.Models.Response;

namespace ListaDeComprasInteligente.Service.Interfaces;

public interface IListaComprasBuilderService
{
    Task<ListaComprasResponse> MontarListaComprasAsync(ListaComprasRequest listaComprasRequest);
}