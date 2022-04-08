namespace ListaDeComprasInteligente.Domain;

public class ListaCompras
{
    public List<Produto> Produtos { get; }

    public ListaCompras()
    {
        Produtos = new List<Produto>();
    }

    
    public void AdicionarProduto(Produto produto) => Produtos.Add(produto);
}