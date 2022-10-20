using PuppeteerSharp;
using PuppeteerBrowser = PuppeteerSharp.Browser;

namespace ListaDeComprasInteligente.Scraper;

public static class Browser
{
    private static PuppeteerBrowser _puppeteerBrowser;

    public static async Task<Page> OpenNewPageAsync()
    {
        if (_puppeteerBrowser is null || _puppeteerBrowser.IsConnected is false)
        {
            await InitializeBrowserAsync();
        }
        return await _puppeteerBrowser!.NewPageAsync();
    }
    

    private static async Task InitializeBrowserAsync()
    {
        try
        {
            _puppeteerBrowser = await Puppeteer.ConnectAsync(
                new ConnectOptions { BrowserWSEndpoint = "ws://chrome:3000" });
        }
        catch
        {
            await DownloadAndLaunchBrowserAsync();
        }
    }


    private static async Task DownloadAndLaunchBrowserAsync()
    {
        var revision = await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
        _puppeteerBrowser = await Puppeteer.LaunchAsync(
            new LaunchOptions
            {
                Headless = true,
                ExecutablePath = revision.ExecutablePath,
                Args = new[] { "--disable-gpu", "--disable-dev-shm-usage", "--no-sandbox", "--disable-setuid-sandbox" }
            });
    }
}