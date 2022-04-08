namespace ListaDeComprasInteligente.Scraper.Models;

public class ScrapRequest
{
    public string NomeProduto { get; }
    public string Url { get; }
    
    public ScrapRequest(string nomeProduto, string url)
    {
        NomeProduto = nomeProduto;
        Url = new Uri(url).ToString();
    }
}