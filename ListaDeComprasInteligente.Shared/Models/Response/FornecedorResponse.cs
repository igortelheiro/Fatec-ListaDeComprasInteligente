using ListaDeComprasInteligente.Shared.Extensions;

namespace ListaDeComprasInteligente.Shared.Models.Response;

public class FornecedorResponse
{
    public string Nome { get; set; }
    public List<ProdutoResponse> Produtos { get; set; }
    public string PrecoTotal { get; set; }

    public FornecedorResponse()
    {
    }
    
    public FornecedorResponse(string nome)
    {
        Nome = nome ?? throw new ArgumentNullException(nameof(nome));
        Produtos = new List<ProdutoResponse>();
        PrecoTotal = ((decimal)0).ToVisualPrice();
    }


    public void AdicionarProduto(ProdutoResponse produto)
    {
        Produtos.Add(produto);
        PrecoTotal = CalcTotalPrice().ToVisualPrice();
    }


    private decimal CalcTotalPrice() =>
        Produtos.Aggregate((decimal)0,
            (total, produto) =>
            {
                total += produto.PrecoUnitario * produto.Quantidade;
                return total;
            });
}