namespace ListaDeComprasInteligente.Domain;

// TODO: View
public class Fornecedor
{
    public string Nome { get; }
    public IEnumerable<Produto> Produtos { get; }
    public decimal PrecoTotal => CalcTotalPrice();
    
    public Fornecedor(string nome, IEnumerable<Produto> produtos)
    {
        Nome = nome ?? throw new ArgumentNullException(nameof(nome));
        Produtos = produtos ?? Array.Empty<Produto>();
    }


    private decimal CalcTotalPrice() =>
        Produtos.Aggregate((decimal)0,
            (total, produto) =>
            {
                var disponibilidade = produto.Disponibilidade.FirstOrDefault(d => d.NomeFornecedor == this.Nome);
                total += disponibilidade?.Preco ?? 0;
                
                return total;
            });


    public void AdicionarProduto(Produto produto) => Produtos.Append(produto);
}
