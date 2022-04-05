using System.IO;
using System.Linq;
using ListaDeComprasInteligente.Service;
using Xunit;

namespace LitaDeComprasInteligente.UnitTests;

public class HtmlParserServiceTests
{
    [Theory]
    [InlineData("Arroz")]
    public void ExtrairProduto(string product)
    {
        var parser = new HtmlParserService();
        var html = File.ReadAllText($"D:\\Projects\\ListaDeComprasInteligente\\LitaDeComprasInteligente.UnitTests\\HTMLs\\{product}Query.html");

        var produtos = parser.ParseHtml(html);

        Assert.Equal(produtos.Count(), 60);
    }
}