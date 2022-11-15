using System.Text;
using System.Text.Encodings.Web;
using ListaDeComprasInteligente.Scraper.Models;
using ListaDeComprasInteligente.Shared.Models.Request;

namespace ListaDeComprasInteligente.Service.Extensions;

public static class RequestExtensions
{
    public static ScrapRequest ToScrapRequest(this ProdutoRequest produtoRequest, Geolocation? geolocation) =>
        new (produtoRequest.BuildUri(), geolocation);


    private static Uri BuildUri(this ProdutoRequest produtoRequest)
    {
        const string baseUrl = "https://www.google.com/search?tbm=shop&q=";
        var encodedProductSearch = produtoRequest.ToString().EncodeToGoogleQuery();

        var stringBuilder = new StringBuilder(baseUrl)
            .Append(encodedProductSearch);

        return new Uri(stringBuilder.ToString());
    }


    private static string EncodeToGoogleQuery(this string query) =>
        UrlEncoder.Default.Encode(query).Replace("%20", "+");
}