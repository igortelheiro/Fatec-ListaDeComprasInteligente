using PuppeteerSharp;
using PuppeteerBrowser = PuppeteerSharp.Browser;

namespace ListaDeComprasInteligente.Scraper;

public static class Browser
{
    private static PuppeteerBrowser _puppeteerBrowser;


    public static async Task<Page> OpenNewPageAsync()
    {
        if (_puppeteerBrowser is null)
        {
            await InitializeBrowserAsync();
        }
        return await _puppeteerBrowser.NewPageAsync();
    }
    

    private static async Task InitializeBrowserAsync()
    {
        await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
        _puppeteerBrowser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true,
            Args = new[] { "--no-sandbox" }
        });
    }
}