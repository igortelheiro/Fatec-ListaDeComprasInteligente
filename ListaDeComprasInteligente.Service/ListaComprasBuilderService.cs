using ListaDeComprasInteligente.Domain;
using ListaDeComprasInteligente.Scraper;
using ListaDeComprasInteligente.Scraper.Models;
using ListaDeComprasInteligente.Service.Extensions;
using ListaDeComprasInteligente.Service.Models;
using ListaDeComprasInteligente.Service.Models.Request;
using ListaDeComprasInteligente.Service.Models.Response;
using System.Globalization;

namespace ListaDeComprasInteligente.Service;

public class ListaComprasBuilderService
{
    private readonly ScraperService _scraperService;

    public ListaComprasBuilderService(ScraperService scraperService) =>
        _scraperService = scraperService;
    
    
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
        var scrapRequest = produto.ToScrapRequest();
        return _scraperService.ScrapAsync(scrapRequest);
    }


    private static Produto BuildProduto(ProdutoRequest produtoRequest, HtmlParseResult htmlParseResult)
    {
        var produto = new Produto(produtoRequest.Nome);
        
        for (var i = 0; i < htmlParseResult.Titulos.Length; i++)
        {
            var tituloAnuncio = htmlParseResult.Titulos[i];
            var preco = htmlParseResult.Precos[i].Replace(',', '.');
            var fornecedor = htmlParseResult.Fornecedores[i];

            var anuncioValido = ValidarAnuncioProduto(produtoRequest, tituloAnuncio);
            if (!anuncioValido)
            {
                continue;
            }
            if (!decimal.TryParse(preco, out var precoProduto))
            {
                continue;
            }
            
            var disponibilidade = new Disponibilidade(tituloAnuncio, fornecedor, precoProduto);
            produto.AdicionarDisponibilidade(disponibilidade);
        }
        
        return produto;
    }


    private static bool ValidarAnuncioProduto(ProdutoRequest produtoRequest, string anuncioProduto)
    {
        var nomeValido = true;
        var palavrasChave = produtoRequest.Nome.Split(' ');
        foreach (var palavra in palavrasChave)
        {
            nomeValido = anuncioProduto.Contains(palavra, StringComparison.InvariantCultureIgnoreCase);
        }
        
        var quantidadeValida = true;
        if (produtoRequest.Quantidade is not null)
        {
            var quantidade = produtoRequest.Quantidade.ToString();
            quantidadeValida = anuncioProduto.Contains(quantidade, StringComparison.InvariantCultureIgnoreCase)
                            || anuncioProduto.Contains(quantidade.Replace(" ", string.Empty), StringComparison.InvariantCultureIgnoreCase);
        }

        var detalhesValidos = true;
        if (produtoRequest.Detalhes is not null)
        {
            var detalhes = produtoRequest.Detalhes.Split(' ');
            foreach (var detalhe in detalhes)
            {
                detalhesValidos = anuncioProduto.Contains(detalhe, StringComparison.InvariantCultureIgnoreCase);
            }
        }
        
        return nomeValido && quantidadeValida && detalhesValidos;
    }
}