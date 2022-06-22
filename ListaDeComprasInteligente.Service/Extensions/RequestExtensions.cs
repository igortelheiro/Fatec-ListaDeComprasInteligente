using System.Text;
using System.Text.Encodings.Web;
using ListaDeComprasInteligente.Scraper.Models;
using ListaDeComprasInteligente.Shared.Models.Request;

namespace ListaDeComprasInteligente.Service.Extensions;

public static class RequestExtensions
{
    private static readonly UrlEncoder UrlEncoder = UrlEncoder.Default;


    public static ScrapRequest ToScrapRequest(this ProdutoRequest produtoRequest, Geolocation? geolocation) =>
        new (produtoRequest.Nome, produtoRequest.BuildUrl(), geolocation);


    private static string BuildUrl(this ProdutoRequest produtoRequest)
    {
        const string baseUrl = "https://www.google.com/search?tbm=shop&q=";
        
        var encodedName = EncodeToGoogleQuery(produtoRequest.Nome);
        var encodedQuantity = EncodeToGoogleQuery(produtoRequest.Quantidade.ToString());
        
        var stringBuilder = new StringBuilder(baseUrl);
        stringBuilder.Append(encodedName);
        stringBuilder.Append(encodedQuantity);
        
        return stringBuilder.ToString();
    }


    private static string EncodeToGoogleQuery(string query) =>
        UrlEncoder.Encode(query).Replace("%20", "+");
}