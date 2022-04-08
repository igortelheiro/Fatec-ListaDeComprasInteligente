using Xunit;

namespace LitaDeComprasInteligente.UnitTests.Services;

public class HtmlParserServiceTests
{
    [Theory]
    [InlineData("Arroz")]
    public void ExtrairProduto(string product)
    {
        // var html = File.ReadAllText($"D:\\Projects\\ListaDeComprasInteligente\\LitaDeComprasInteligente.UnitTests\\HTMLs\\{product}Query.html");
        //
        // var produto = HtmlParserService.ParseProductHtml(product, html);
        //
        // Assert.Equal(60, produto.Disponibilidade.Count);
    }
}