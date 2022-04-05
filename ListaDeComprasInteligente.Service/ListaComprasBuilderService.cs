using ListaDeComprasInteligente.Domain;
using ListaDeComprasInteligente.Scraper;
using ListaDeComprasInteligente.Service.Extensions;
using ListaDeComprasInteligente.Service.Models;

namespace ListaDeComprasInteligente.Service;

public class ListaComprasBuilderService
{
    private readonly HtmlParserService _htmlParserService;
    private readonly ScraperService _scraperService;

    public ListaComprasBuilderService(HtmlParserService htmlParserService, ScraperService scraperService)
    {
        _htmlParserService = htmlParserService;
        _scraperService = scraperService;
    }
    
    
    public async Task<ListaCompras> MontarListaAsync(ListaComprasRequest listaComprasRequest)
    {
        var listaDeCompras = new ListaCompras();
        var scrapResult = await ScrapProductsAsync(listaComprasRequest);
        
        foreach (var html in scrapResult)
        {
            var produtos = _htmlParserService.ParseHtml(html);
            foreach (var produto in produtos)
            {
                foreach (var fornecedor in produto.Disponibilidade)
                {
                    listaDeCompras.AdicionarProduto(produto, fornecedor.NomeFornecedor);
                }
            }
        }

        return listaDeCompras;
    }


    private async Task<IEnumerable<string>> ScrapProductsAsync(ListaComprasRequest listaComprasRequest)
    {
        var htmls = new List<string>();
        
        await Parallel.ForEachAsync(listaComprasRequest.Produtos, async (produto, cancellationToken) =>
        {
            var scrapRequest = produto.ToScrapRequest();
            var scrapResult = await _scraperService.ScrapAsync(scrapRequest);
            htmls.Add(scrapResult);
        });

        return htmls;
    }
}