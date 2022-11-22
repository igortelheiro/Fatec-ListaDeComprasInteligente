using System.Text;
using System.Text.Encodings.Web;
using ListaDeComprasInteligente.Scraper.Models;
using ListaDeComprasInteligente.Shared.Models.Request;

namespace ListaDeComprasInteligente.Service.Extensions;

public static class RequestExtensions
{
    public static ScrapRequest ToScrapRequest(this ProdutoRequest produtoRequest, Geolocation? geolocation) =>
        new(produtoRequest.GetUrisToScrap(), geolocation);


    private static List<Uri> GetUrisToScrap(this ProdutoRequest produtoRequest)
    {
        const string baseUrl = "https://www.google.com/search?gl=br&tbm=shop&q=";
        var query = GetEncodedQuery(produtoRequest);

        var stringBuilder = new StringBuilder(baseUrl).Append(query);

        return BuildPagesUris(ref stringBuilder);
    }


    private static string GetEncodedQuery(ProdutoRequest produtoRequest)
    {
        var query = produtoRequest.ToString();
        var encodedQuery = UrlEncoder.Default.Encode(query).Replace("%20", "+");

        return encodedQuery;
    }


    private static List<Uri> BuildPagesUris(ref StringBuilder sb)
    {
        const string paginationQuery = "?start=";
        const int productsPerPage = 60;
        var paginator = 0;
        
        var uris = new List<Uri>();

        foreach (var _ in Enumerable.Range(0, 3))
        {
            sb.Append(paginationQuery + productsPerPage.ToString());
            paginator += productsPerPage;

            var newUri = new Uri(sb.ToString());
            uris.Add(newUri);
        }

        return uris;
    }
}