namespace ListaDeComprasInteligente.Scraper.Models;

public class ScrapResult
{
    public string Html { get; }

    public ScrapResult(string html)
    {
        Html = html;
    }
}