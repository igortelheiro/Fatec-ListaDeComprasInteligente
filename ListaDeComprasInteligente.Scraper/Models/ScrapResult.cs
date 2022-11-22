namespace ListaDeComprasInteligente.Scraper.Models;

public class ScrapResult
{
    public List<string> Titulos { get; init; }
    public List<string> Precos { get; init; }
    public List<string> Fornecedores { get; init; }

    public ScrapResult()
    {
        Titulos = new();
        Precos = new();
        Fornecedores = new();
    }

    public ScrapResult(List<string> titulos, List<string> precos, List<string> fornecedores)
    {
        Titulos = titulos;
        Precos = precos;
        Fornecedores = fornecedores;
    }


    public void AddScrap(ScrapResult scrap)
    {
        Titulos.AddRange(scrap.Titulos);
        Precos.AddRange(scrap.Precos);
        Fornecedores.AddRange(scrap.Fornecedores);
    }
}