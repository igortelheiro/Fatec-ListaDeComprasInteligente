namespace ListaDeComprasInteligente.Domain;

public class ListaCompras
{
    public List<Fornecedor> Fornecedores { get; }
    public Fornecedor FornecedorMaisCompetitivo { get; private set; }

    public ListaCompras()
    {
        Fornecedores = new List<Fornecedor>();
    }

    
    public void AdicionarProduto(Produto produto, string nomeFornecedor)
    {
        var fornecedor = Fornecedores.FirstOrDefault(f => f.Nome == nomeFornecedor);
        if (fornecedor is not null)
        {
            fornecedor.AdicionarProduto(produto);
            return;
        }
        
        var newFornecedor = new Fornecedor(nomeFornecedor, new Produto[] { produto });
        Fornecedores.Add(newFornecedor);
        AtualizarFornecedorMaisCompetitivo();
    }

    private void AtualizarFornecedorMaisCompetitivo() =>
        FornecedorMaisCompetitivo = Fornecedores.MinBy(f => f.PrecoTotal);
}