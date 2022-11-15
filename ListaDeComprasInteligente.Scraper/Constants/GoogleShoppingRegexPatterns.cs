namespace ListaDeComprasInteligente.Scraper.Constants;

public static class GoogleShoppingRegexPatterns
{
    public const string Product = "(?:class=\"i0X6df\">)(.+?)(?:class=\"sh-pl__log\">)";
    public const string ProductTitle = "(?:class=\"tAxDx\">)(.+?)(?:</h3>)";
    public const string ProductPrice = "(?:class=\"a8Pemb OFFNJ\">)(.+?)(?:</span>)";
    public const string ProductSuplier = "(?:class=\"aULzUe IuHnof\">)(?:<.+?</.+?>)?(.+?)(?:</div>)";
}