using System.Text.RegularExpressions;
using ListaDeComprasInteligente.Domain;
using ListaDeComprasInteligente.Service.Constants;

namespace ListaDeComprasInteligente.Service;

public class HtmlParserService
{
    public IEnumerable<Produto> ParseHtml(string html)
    {
        var productTitles = ExtractValues(html, GoogleShoppingRegexPatterns.ProductTitle);
        var productPrices = ExtractValues(html, GoogleShoppingRegexPatterns.ProductPrice);
        var productSupliers = ExtractValues(html, GoogleShoppingRegexPatterns.ProductSuplier);

        var produtos = new List<Produto>();
        
        for (var i = 0; i < productTitles.Count(); i++)
        {
            var title = productTitles[i];
            var price = productPrices[i];
            var suplier = productSupliers[i];
            
            var newDisponibilidade = new Disponibilidade(suplier, decimal.Parse(price));
            var newProduto = new Produto(title, new[] { newDisponibilidade });
            
            produtos.Add(newProduto);
        }
        
        return produtos;
    }


    private static string[] ExtractValues(string html, string regexPattern) =>
        new Regex(regexPattern)
            .Matches(html)
            .Select(m => m.Groups.Values.ToArray()[1])
            .Select(g => g.Value.Replace("R$&nbsp;", string.Empty))
            .ToArray();
}