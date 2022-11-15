namespace ListaDeComprasInteligente.Scraper.Models;

public readonly record struct ScrapRequest(Uri Uri, Geolocation? Geolocation);