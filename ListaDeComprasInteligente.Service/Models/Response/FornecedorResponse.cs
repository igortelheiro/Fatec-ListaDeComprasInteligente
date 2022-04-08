using ListaDeComprasInteligente.Service.Extensions;

namespace ListaDeComprasInteligente.Service.Models.Response;

public class FornecedorResponse
{
    public string Nome { get; }
    public List<ProdutoResponse> Produtos { get; }
    public string PrecoTotal { get; private set; }
    
    public FornecedorResponse(string nome)
    {
        Nome = nome ?? throw new ArgumentNullException(nameof(nome));
        Produtos = new List<ProdutoResponse>();
        // PrecoTotal = $"R$ {CalcTotalPrice(produtos):0.00}";
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