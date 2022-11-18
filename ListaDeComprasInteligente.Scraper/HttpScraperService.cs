using ListaDeComprasInteligente.Scraper.Constants;
using ListaDeComprasInteligente.Scraper.Interfaces;
using ListaDeComprasInteligente.Scraper.Models;
using ListaDeComprasInteligente.Shared.Exceptions;
using System.Text.RegularExpressions;

namespace ListaDeComprasInteligente.Scraper;

public class HttpScraperService : IScraperService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HttpScraperService(IHttpClientFactory httpClientFactory)
    {
        ArgumentNullException.ThrowIfNull(httpClientFactory);

        _httpClientFactory = httpClientFactory;
    }

    public async Task<ScrapResult> ScrapAsync(ScrapRequest scrapRequest)
    {
        var html = await GetPageContentAsync(scrapRequest.Uri);
        if (string.IsNullOrEmpty(html))
        {
            throw new ServiceException("O scrap não retornou um arquivo html");
        }

        var scrapResult = ParseHtml(html);
        return scrapResult;
    }


    private async Task<string> GetPageContentAsync(Uri uri)
    {
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36 Edg/107.0.1418.35");
        
        var searchResult = await httpClient.GetAsync(uri);
        return await searchResult.Content.ReadAsStringAsync();
    }


    public static ScrapResult ParseHtml(string html)
    {
        var anuncios = ExtractAnuncios(html, GoogleShoppingRegexPatterns.Product);

        List<string> titulos = new();
        List<string> precos = new();
        List<string> fornecedores = new();

        foreach(var anuncio in anuncios)
        {
            var titulo = ExtractValue(anuncio, GoogleShoppingRegexPatterns.ProductTitle);
            var preco = ExtractValue(anuncio, GoogleShoppingRegexPatterns.ProductPrice);
            var fornecedor = ExtractValue(anuncio, GoogleShoppingRegexPatterns.ProductSuplier);
            
            titulos.Add(titulo);
            precos.Add(preco);
            fornecedores.Add(fornecedor);
        }

        var anyValueFound = titulos.Count > 0 || precos.Count > 0 || fornecedores.Count > 0;
        if (!anyValueFound)
        {
            throw new ServiceException("Nenhum valor foi extraído do html");
        }

        var isParseValid = titulos.Count == precos.Count && titulos.Count == fornecedores.Count;
        if (!isParseValid)
        {
            throw new ServiceException("Quantidade inválida de valores extraídos do html");
        }

        return new ScrapResult(titulos.ToArray(), precos.ToArray(), fornecedores.ToArray());
    }


    private static string[] ExtractAnuncios(string html, string regexPattern) =>
        new Regex(regexPattern)
            .Matches(html)
            .Select(m => m.Groups.Values.ToArray()[1])
            .Select(g => g.Value)
            .ToArray();


    private static string ExtractValue(string anuncio, string regexPattern) =>
        new Regex(regexPattern)
            .Matches(anuncio)
            .Select(m => m.Groups.Values.ToArray()[1])
            .Select(g => g.Value.Replace("R$", string.Empty).TrimStart())
            .FirstOrDefault(string.Empty);
}