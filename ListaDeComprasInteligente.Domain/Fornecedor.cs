using ListaDeComprasInteligente.Domain.ValueObjects;

namespace ListaDeComprasInteligente.Domain;

public class Fornecedor
{
    public string Nome { get; set; }
    public IEnumerable<Produto> Produtos { get; set; }
    public decimal PrecoTotal { get; set; }

    public Fornecedor(string nome)
    {
        ArgumentNullException.ThrowIfNull(nome);

        Nome = nome;
        Produtos = Array.Empty<Produto>();
        PrecoTotal = 0m;
    }


    public void AdicionarProduto(Produto produto)
    {
        Produtos = Produtos.Concat(new Produto[] { produto });
        PrecoTotal = CalcTotalPrice();
    }


    private decimal CalcTotalPrice() =>
        Produtos.Aggregate(0m,
            (total, produto) =>
            {
                total += produto.PrecoUnitario * produto.Quantidade;
                return total;
            });
}
