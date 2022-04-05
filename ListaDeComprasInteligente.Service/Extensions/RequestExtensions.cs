using System.Text;
using System.Text.Encodings.Web;
using ListaDeComprasInteligente.Scraper.Models;
using ListaDeComprasInteligente.Service.Models;

namespace ListaDeComprasInteligente.Service.Extensions;

public static class RequestExtensions
{
    private static readonly UrlEncoder UrlEncoder = UrlEncoder.Default;
    
    
    public static ScrapRequest ToScrapRequest(this ProdutoRequest produtoRequest) =>
        new()
        {
            Url = produtoRequest.BuildUrl()
        };


    private static string BuildUrl(this ProdutoRequest produtoRequest)
    {
        const string baseUrl = "https://www.google.com/search?tbm=shop&q=";
        var encodedQuery = EncodeGoogleQuery(produtoRequest.PalavrasChave);
        
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(baseUrl);
        stringBuilder.Append(encodedQuery);
        
        return stringBuilder.ToString();
    }


    private static string EncodeGoogleQuery(string query) =>
        UrlEncoder.Encode(query).Replace("%20", "+");
}