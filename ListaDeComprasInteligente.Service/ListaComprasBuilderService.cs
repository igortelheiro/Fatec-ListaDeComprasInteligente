using System.Globalization;
using ListaDeComprasInteligente.Domain;
using ListaDeComprasInteligente.Scraper;
using ListaDeComprasInteligente.Scraper.Models;
using ListaDeComprasInteligente.Service.Extensions;
using ListaDeComprasInteligente.Service.Models;
using ListaDeComprasInteligente.Shared.Models.Request;
using ListaDeComprasInteligente.Shared.Models.Response;
using Microsoft.AspNetCore.Http;

namespace ListaDeComprasInteligente.Service;

public class ListaComprasBuilderService
{
    private readonly ScraperService _scraperService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ListaComprasBuilderService(ScraperService scraperService, IHttpContextAccessor httpContextAccessor)
    {
        _scraperService = scraperService;
        _httpContextAccessor = httpContextAccessor;
    }
    
    
    public async Task<ListaComprasResponse> MontarListaComprasAsync(ListaComprasRequest listaComprasRequest)
    {
        var listaDeCompras = new ListaCompras();

        await Parallel.ForEachAsync(listaComprasRequest.Produtos, async (request, token) =>
        {
            var scrapResult = await ScrapProductAsync(request);
            var htmlParseResult = HtmlParser.ParseHtml(scrapResult.Html);
            var anuncios = ExtrairAnuncios(request, htmlParseResult);

            listaDeCompras.AdicionarProduto(request.Nome);
            anuncios.ForEach(a => listaDeCompras.AdicionarAnuncio(request.Nome, a));
        });

        return new ListaComprasResponse(listaDeCompras, listaComprasRequest);;
    }


    private Task<ScrapResult> ScrapProductAsync(ProdutoRequest produto)
    {
        var scrapRequest = produto.ToScrapRequest(_httpContextAccessor.GetGeolocation());
        return _scraperService.ScrapAsync(scrapRequest);
    }


    // TODO: levar lógica pra dentro do Domain Produto
    private static List<Anuncio> ExtrairAnuncios(ProdutoRequest produtoRequest, HtmlParseResult htmlParseResult)
    {
        var anuncios = new List<Anuncio>();

        for (var i = 0; i < htmlParseResult.Titulos.Length; i++)
        {
            var tituloAnuncio = htmlParseResult.Titulos[i];
            var precoAnuncio = htmlParseResult.Precos[i].Replace(',', '.');
            var fornecedor = htmlParseResult.Fornecedores[i];

            var anuncioValido = ValidarAnuncioProduto(tituloAnuncio, produtoRequest);
            if (!anuncioValido) continue;

            var precoValido = decimal.TryParse(precoAnuncio, NumberStyles.Currency, CultureInfo.InvariantCulture, out var preco);
            if (!precoValido) continue;
            
            var anuncio = new Anuncio(tituloAnuncio, fornecedor, preco);
            anuncios.Add(anuncio);
        }
        
        return anuncios;
    }


    // TODO: levar lógica pra dentro do Domain Produto
    private static bool ValidarAnuncioProduto(string anuncioProduto, ProdutoRequest produtoRequest)
    {
        var palavrasChave = produtoRequest.ToString().Split(' ');
        var anuncioValido = palavrasChave.All(palavra => anuncioProduto.Contains(palavra, StringComparison.InvariantCultureIgnoreCase));

        return anuncioValido;
    }
}