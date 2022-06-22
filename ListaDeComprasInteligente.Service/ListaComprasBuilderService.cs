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

    public ListaComprasBuilderService(ScraperService scraperService)
    {
        _scraperService = scraperService;
    }

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
            var produto = BuildProduto(request, htmlParseResult);
            listaDeCompras.AdicionarProduto(produto);
        });

        return new ListaComprasResponse(listaDeCompras, listaComprasRequest);;
    }


    private Task<ScrapResult> ScrapProductAsync(ProdutoRequest produto)
    {
        var scrapRequest = produto.ToScrapRequest(_httpContextAccessor?.GetGeolocation());
        return _scraperService.ScrapAsync(scrapRequest);
    }


    private static Produto BuildProduto(ProdutoRequest produtoRequest, HtmlParseResult htmlParseResult)
    {
        var produto = new Produto(produtoRequest.Nome);
        
        for (var i = 0; i < htmlParseResult.Titulos.Length; i++)
        {
            var tituloAnuncio = htmlParseResult.Titulos[i];
            var precoAnuncio = htmlParseResult.Precos[i].Replace(',', '.');
            var fornecedor = htmlParseResult.Fornecedores[i];

            var anuncioValido = ValidarAnuncioProduto(tituloAnuncio, produtoRequest);
            if (!anuncioValido) continue;

            var precoValido = decimal.TryParse(precoAnuncio, NumberStyles.Currency, CultureInfo.InvariantCulture, out var preco);
            if (!precoValido) continue;
            
            var disponibilidade = new Disponibilidade(tituloAnuncio, fornecedor, preco);
            produto.AdicionarDisponibilidade(disponibilidade);
        }
        
        return produto;
    }


    private static bool ValidarAnuncioProduto(string anuncioProduto, ProdutoRequest produtoRequest)
    {
        var palavrasChave = produtoRequest.Nome.Split(' ');
        var nomeValido = palavrasChave.All(palavra => anuncioProduto.Contains(palavra, StringComparison.InvariantCultureIgnoreCase));
        
        var quantidade = produtoRequest.Quantidade.ToString();
        var quantidadeValida = anuncioProduto.Contains(quantidade, StringComparison.InvariantCultureIgnoreCase)
                                 || anuncioProduto.Contains(quantidade.Replace(" ", string.Empty), StringComparison.InvariantCultureIgnoreCase);

        return nomeValido && quantidadeValida;
    }
}