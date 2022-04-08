using System.Text;
using System.Text.Encodings.Web;
using ListaDeComprasInteligente.Scraper.Models;
using ListaDeComprasInteligente.Service.Models.Request;

namespace ListaDeComprasInteligente.Service.Extensions;

public static class RequestExtensions
{
    private static readonly UrlEncoder UrlEncoder = UrlEncoder.Default;


    public static ScrapRequest ToScrapRequest(this ProdutoRequest produtoRequest) =>
        new ScrapRequest(produtoRequest.Nome, produtoRequest.BuildUrl());


    private static string BuildUrl(this ProdutoRequest produtoRequest)
    {
        const string baseUrl = "https://www.google.com/search?tbm=shop&q=";
        
        var encodedName = EncodeToGoogleQuery(produtoRequest.Nome);
        
        var encodedQuantity = string.Empty;
        if (produtoRequest.Quantidade is not null)
        {
            encodedQuantity = EncodeToGoogleQuery(produtoRequest.Quantidade.ToString());
        }
        
        var encodedDetails = string.Empty;
        if (produtoRequest.Detalhes is not null)
        {
            encodedDetails = EncodeToGoogleQuery(produtoRequest.Detalhes);
        }
        
        var stringBuilder = new StringBuilder(baseUrl);
        stringBuilder.Append(encodedName);
        stringBuilder.Append(encodedQuantity);
        stringBuilder.Append(encodedDetails);
        
        return stringBuilder.ToString();
    }


    private static string EncodeToGoogleQuery(string query) =>
        UrlEncoder.Encode(query).Replace("%20", "+");
}