using ListaDeComprasInteligente.Domain.ValueObjects;

namespace ListaDeComprasInteligente.Domain;

public class Fornecedor
{
    public string Nome { get; set; }
    public List<Produto> Produtos { get; set; }
    public decimal PrecoTotal { get; set; }

    public Fornecedor(string nome)
    {
        ArgumentNullException.ThrowIfNull(nome);

        Nome = nome;
        Produtos = new();
        PrecoTotal = 0m;
    }


    public void AdicionarProduto(Produto produto)
    {
        Produtos.Add(produto);
        CalcTotalPrice();
    }


    private decimal CalcTotalPrice() =>
        PrecoTotal = Produtos.Aggregate(0m,
                                       (total, produto) =>
                                       {
                                           total += produto.PrecoUnitario * produto.Quantidade;
                                           return total;
                                       });
}
