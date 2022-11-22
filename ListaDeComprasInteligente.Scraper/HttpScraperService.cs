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
        var scrapResult = new ScrapResult();

        await Parallel.ForEachAsync(scrapRequest.Uris, async (uri, token) =>
        {
            var html = await GetPageContentAsync(uri);
            ValidarScrap(html);

            var scrap = BuildScrapResult(html);
            scrapResult.AddScrap(scrap);
        });


        return scrapResult;
    }


    // TODO: Test
    public static ScrapResult BuildScrapResult(string html)
    {
        var anuncios = ExtrairAnuncios(html, GoogleShoppingRegexPatterns.Anuncio);

        var titulos = new List<string>();
        var precos = new List<string>();
        var fornecedores = new List<string>();

        foreach(var anuncio in anuncios)
        {
            var titulo = ExtrairValor(GoogleShoppingRegexPatterns.TituloAnuncio, anuncio);
            var preco = ExtrairValor(GoogleShoppingRegexPatterns.Preco, anuncio);
            var fornecedor = ExtrairValor(GoogleShoppingRegexPatterns.Fornecedor, anuncio);
            
            titulos.Add(titulo);
            precos.Add(preco);
            fornecedores.Add(fornecedor);
        }

        ValidarExtracao(titulos, precos, fornecedores);

        return new ScrapResult(titulos, precos, fornecedores);
    }


    #region Private
    private async Task<string> GetPageContentAsync(Uri uri)
    {
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36 Edg/107.0.1418.35");

        var searchResult = await httpClient.GetAsync(uri);
        return await searchResult.Content.ReadAsStringAsync();
    }
    
    
    private static void ValidarScrap(string html)
    {
        if (string.IsNullOrEmpty(html))
            throw new ServiceException("O scrap não retornou um arquivo html");
    }


    private static string[] ExtrairAnuncios(string html, string regexPattern) =>
        new Regex(regexPattern)
            .Matches(html)
            .Select(match => match.Groups.Values.ToArray()[1])
            .Select(group => group.Value)
            .ToArray();


    private static string ExtrairValor(string regexPattern, string anuncio) =>
        new Regex(regexPattern)
            .Matches(anuncio)
            .Select(match => match.Groups.Values.ToArray()[1])
            .Select(group => group.Value.Replace("R$", string.Empty).TrimStart())
            .FirstOrDefault(string.Empty);


    private static void ValidarExtracao(List<string> titulos, List<string> precos, List<string> fornecedores)
    {
        var anyValueFound = titulos.Count > 0 || precos.Count > 0 || fornecedores.Count > 0;
        if (anyValueFound is not true)
            throw new ServiceException("Nenhum valor foi extraído do html");

        var isParseValid = titulos.Count == precos.Count && titulos.Count == fornecedores.Count;
        if (isParseValid is not true)
            throw new ServiceException("Quantidade inválida de valores extraídos do html");
    }
    #endregion
}