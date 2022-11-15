using ListaDeComprasInteligente.Scraper.Models;

namespace ListaDeComprasInteligente.Scraper.Interfaces;

public interface IScraperService
{
    Task<ScrapResult> ScrapAsync(ScrapRequest scrapRequest);
}
