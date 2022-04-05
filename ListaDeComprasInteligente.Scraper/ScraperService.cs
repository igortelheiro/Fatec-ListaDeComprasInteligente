using ListaDeComprasInteligente.Scraper.Models;
using PuppeteerSharp;

namespace ListaDeComprasInteligente.Scraper;

public class ScraperService
{
    public async Task<string> ScrapAsync(ScrapRequest scrapRequest)
    {
        await using var page = await Browser.OpenNewPageAsync();
        
        await page.GoToAsync(scrapRequest.Url, WaitUntilNavigation.Networkidle2);
        var html = await page.GetContentAsync();
        
        await page.CloseAsync();
        
        return html;
    }
}