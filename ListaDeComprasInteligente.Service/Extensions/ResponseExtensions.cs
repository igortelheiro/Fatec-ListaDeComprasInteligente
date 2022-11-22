using ListaDeComprasInteligente.Domain;
using ListaDeComprasInteligente.Domain.ValueObjects;
using ListaDeComprasInteligente.Shared.Models.Response;

namespace ListaDeComprasInteligente.Service.Extensions;

public static class ResponseExtensions
{
    public static ListaComprasResponse ToResponse(this ListaCompras listaCompras)
    {
        var fornecedoresResponse = listaCompras.Fornecedores.Select(f => f.ToResponse());
        var fornecedorMaisCompetitivo = listaCompras.FornecedorMaisCompetitivo?.ToResponse();
        return new ListaComprasResponse(fornecedoresResponse, fornecedorMaisCompetitivo);
    }


    private static FornecedorResponse ToResponse(this Fornecedor fornecedor)
    {
        var produtosResponse = fornecedor.Produtos.Select(p => p.ToResponse());
        return new(fornecedor.Nome, produtosResponse, fornecedor.PrecoTotal.ToVisualPrice());
    }


    private static ProdutoResponse ToResponse(this Produto produto) =>
        new(produto.Nome, produto.Quantidade, produto.PrecoUnitario);


    private static string ToVisualPrice(this decimal price) => $"R${price:0.00}";
}