namespace ListaDeComprasInteligente.Scraper.Models;

public class ScrapRequest
{
    public string NomeProduto { get; }
    public string Url { get; }
    public Geolocation? Geolocation { get; }
    
    public ScrapRequest(string nomeProduto, string url, Geolocation? geolocation)
    {
        NomeProduto = nomeProduto;
        Url = new Uri(url).ToString();
        Geolocation = geolocation;
    }
}

public class Geolocation
{
    public decimal Latitude { get; }
    public decimal Longitude { get; }

    public Geolocation(decimal latitude, decimal longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
}