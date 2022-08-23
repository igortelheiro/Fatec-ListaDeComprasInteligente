using ListaDeComprasInteligente.Shared.Models.Request;
using ListaDeComprasInteligente.Shared.Models.Response;
using Microsoft.AspNetCore.Components;

namespace ListaDeComprasInteligente.WebApp.Components;

public partial class ResultadoBase : ComponentBase
{
    private ListaComprasResponse _listaComprasResponse;

    [Parameter] public ListaComprasResponse ListaComprasResponse
    {
        get => _listaComprasResponse;
        set
        {
            _listaComprasResponse = value;
            _listaComprasResponse.Fornecedores.OrderBy(f => f.PrecoTotal).ToList();
        }
    }
    [Parameter] public EventCallback OnCloseResult { get; set; }

    protected bool IsComparingProducts = false;

    protected ResultadoBase()
    {
       _listaComprasResponse ??= new();
    }


    protected Task CloseResult() => OnCloseResult.InvokeAsync();

    protected void CompareProducts() => IsComparingProducts = true;
    protected void CloseProductComparison() => IsComparingProducts = false;
}
