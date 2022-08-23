namespace ListaDeComprasInteligente.Domain;

public class ListaCompras
{
    public List<Produto> Produtos { get; }

    public ListaCompras()
    {
        Produtos = new List<Produto>();
    }


    // TODO: tirar média dos valores e excluir grandes disparidades
    public void AdicionarProduto(Produto produto) => Produtos.Add(produto);
}