namespace ListaDeComprasInteligente.Domain;

public class Disponibilidade
{
    public string TituloAnuncio { get; }
    public string NomeFornecedor { get; }
    public decimal Preco { get; }

    public Disponibilidade(string tituloAnuncio, string nomeFornecedor, decimal preco)
    {
        TituloAnuncio = tituloAnuncio;
        NomeFornecedor = nomeFornecedor;
        Preco = preco;
    }
}