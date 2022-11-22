namespace ListaDeComprasInteligente.Scraper.Models;

public readonly record struct ScrapRequest(IEnumerable<Uri> Uris, Geolocation? Geolocation);