namespace ListaDeComprasInteligente.Service.Constants;

public static class GoogleShoppingRegexPatterns
{
    public const string ProductTitle = "(?:class=\"Xjkr3b\">)(.+?)(?:</h4>)";
    public const string ProductPrice = "(?:class=\"a8Pemb OFFNJ\">)(.+?)(?:</span>)";
    public const string ProductSuplier = "(?:class=\"aULzUe IuHnof\">)(?:<.+?</.+?>)?(.+?)(?:</div>)";
}