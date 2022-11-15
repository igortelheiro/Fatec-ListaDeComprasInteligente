using ListaDeComprasInteligente.Domain;
using ListaDeComprasInteligente.Domain.ValueObjects;
using ListaDeComprasInteligente.Scraper.Interfaces;
using ListaDeComprasInteligente.Scraper.Models;
using ListaDeComprasInteligente.Service.Extensions;
using ListaDeComprasInteligente.Service.Interfaces;
using ListaDeComprasInteligente.Shared.Models.Request;
using ListaDeComprasInteligente.Shared.Models.Response;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace ListaDeComprasInteligente.Service;

public class ListaComprasBuilderService : IListaComprasBuilderService
{
    private readonly IScraperService _scraperService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ListaComprasBuilderService(IScraperService scraperService, IHttpContextAccessor httpContextAccessor)
    {
        _scraperService = scraperService;
        _httpContextAccessor = httpContextAccessor;
    }


    public async Task<ListaComprasResponse> MontarListaComprasAsync(ListaComprasRequest listaComprasRequest)
    {
        var listaDeCompras = new ListaCompras(listaComprasRequest);

        await Parallel.ForEachAsync(listaComprasRequest.Produtos, async (request, token) =>
        {
            var scrapResult = await ScrapProductAsync(request);
            var anuncios = ExtrairAnuncios(request, scrapResult);

            listaDeCompras.AdicionarProduto(request.Nome, anuncios);
        });

        listaDeCompras.OrganizarProdutosPorFornecedor();
        listaDeCompras.EncontrarFornecedorMaisCompetitivo();
        return listaDeCompras.ToResponse(listaComprasRequest);
    }


    private Task<ScrapResult> ScrapProductAsync(ProdutoRequest produto)
    {
        var scrapRequest = produto.ToScrapRequest(_httpContextAccessor.GetGeolocation());
        return _scraperService.ScrapAsync(scrapRequest);
    }


    private static List<Anuncio> ExtrairAnuncios(ProdutoRequest produtoRequest, ScrapResult scrapResult)
    {
        var anuncios = new List<Anuncio>();

        for (var i = 0; i < scrapResult.Titulos.Length; i++)
        {
            var tituloAnuncio = scrapResult.Titulos[i];
            var precoAnuncio = scrapResult.Precos[i].Replace(',', '.');
            var fornecedor = scrapResult.Fornecedores[i];

            var anuncioValido = ValidarAnuncioProduto(tituloAnuncio, produtoRequest);
            if (!anuncioValido) continue;

            var precoValido = decimal.TryParse(precoAnuncio, NumberStyles.Currency, CultureInfo.InvariantCulture, out var preco);
            if (!precoValido) continue;

            var anuncio = new Anuncio(tituloAnuncio, fornecedor, preco);
            anuncios.Add(anuncio);
        }

        return anuncios;
    }


    private static bool ValidarAnuncioProduto(string anuncioProduto, ProdutoRequest produtoRequest)
    {
        var palavrasChave = produtoRequest.ToString().Split(' ');
        var anuncioValido = palavrasChave.All(palavra => anuncioProduto.Contains(palavra, StringComparison.InvariantCultureIgnoreCase));

        return anuncioValido;
    }
}