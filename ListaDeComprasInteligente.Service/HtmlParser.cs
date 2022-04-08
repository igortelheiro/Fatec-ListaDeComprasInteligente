using System.Text.RegularExpressions;
using ListaDeComprasInteligente.Service.Constants;
using ListaDeComprasInteligente.Service.Models;

namespace ListaDeComprasInteligente.Service;

public static class HtmlParser
{
    public static HtmlParseResult ParseHtml(string html)
    {
        var titulos = ExtractValues(html, GoogleShoppingRegexPatterns.ProductTitle);
        var precos = ExtractValues(html, GoogleShoppingRegexPatterns.ProductPrice);
        var fornecedores = ExtractValues(html, GoogleShoppingRegexPatterns.ProductSuplier);

        var isParseValid = titulos.Length == precos.Length && titulos.Length == fornecedores.Length;
        if (!isParseValid)
        {
            throw new Exception("Erro ao analisar html");
        }
        
        return new HtmlParseResult(titulos, precos, fornecedores);
    }


    private static string[] ExtractValues(string html, string regexPattern) =>
        new Regex(regexPattern)
            .Matches(html)
            .Select(m => m.Groups.Values.ToArray()[1])
            .Select(g => g.Value.Replace("R$&nbsp;", string.Empty))
            .ToArray();
}