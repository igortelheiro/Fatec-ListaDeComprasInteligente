namespace ListaDeComprasInteligente.Service.Models;

public class HtmlParseResult
{
    public string[] Titulos { get; }
    public string[] Precos { get; }
    public string[] Fornecedores { get; }

    public HtmlParseResult(string[] titulos, string[] precos, string[] fornecedores)
    {
        Titulos = titulos;
        Precos = precos;
        Fornecedores = fornecedores;
    }
}