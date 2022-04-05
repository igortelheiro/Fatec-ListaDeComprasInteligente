namespace ListaDeComprasInteligente.Domain;

public class Disponibilidade
{
    public string NomeFornecedor { get; }
    public decimal Preco { get; }

    public Disponibilidade(string nomeFornecedor, decimal preco)
    {
        NomeFornecedor = nomeFornecedor;
        Preco = preco;
    }
}