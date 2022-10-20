namespace ListaDeComprasInteligente.Domain;

public class Anuncio
{
    public string Titulo { get; }
    public string NomeFornecedor { get; }
    public decimal Preco { get; }

    public Anuncio(string tituloAnuncio, string nomeFornecedor, decimal preco)
    {
        Titulo = tituloAnuncio;
        NomeFornecedor = nomeFornecedor;
        Preco = preco;
    }
}