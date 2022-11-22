namespace ListaDeComprasInteligente.Scraper.Constants;

public static class GoogleShoppingRegexPatterns
{
    public const string Anuncio = "(?:class=\"i0X6df\">)(.+?)(?:class=\"sh-pl__log\">)";
    public const string TituloAnuncio = "(?:class=\"tAxDx\">)(.+?)(?:</h3>)";
    public const string Preco = "(?:class=\"a8Pemb OFFNJ\">)(.+?)(?:</span>)";
    public const string Fornecedor = "(?:class=\"aULzUe IuHnof\">)(?:<.+?</.+?>)?(.+?)(?:</div>)";
}