using ListaDeComprasInteligente.Shared.Extensions;

namespace ListaDeComprasInteligente.Shared.Models.Response;

public class FornecedorResponse
{
    public string Nome { get; set; }
    public IEnumerable<ProdutoResponse> Produtos { get; set; }
    public string PrecoTotal { get; set; }

    public FornecedorResponse()
    {
    }
    
    public FornecedorResponse(string nome)
    {
        Nome = nome ?? throw new ArgumentNullException(nameof(nome));
        Produtos = Array.Empty<ProdutoResponse>();
        PrecoTotal = 0m.ToVisualPrice();
    }


    public void AdicionarProduto(ProdutoResponse produto)
    {
        Produtos = Produtos.Concat(new ProdutoResponse[] { produto });
        PrecoTotal = CalcTotalPrice().ToVisualPrice();
    }


    private decimal CalcTotalPrice() =>
        Produtos.Aggregate(0m,
            (total, produto) =>
            {
                total += produto.PrecoUnitario * produto.Quantidade;
                return total;
            });
}