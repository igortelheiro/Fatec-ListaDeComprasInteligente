using System.Globalization;
using ListaDeComprasInteligente.Scraper.Models;
using Microsoft.AspNetCore.Http;

namespace ListaDeComprasInteligente.Service.Extensions;

public static class HttpContextAccessorExtensions
{
    public static Geolocation? GetGeolocation(this IHttpContextAccessor httpContextAccessor)
    {
        var hasLatitude = httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Latitude", out var latitudeHeader);
        var hasLongitude = httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Longitude", out var longitudeHeader);

        if (!hasLatitude || !hasLongitude) return null;
        
        var latitude = Convert.ToDecimal(latitudeHeader.FirstOrDefault(), CultureInfo.InvariantCulture);
        var longitude = Convert.ToDecimal(longitudeHeader.FirstOrDefault(), CultureInfo.InvariantCulture);
        return new Geolocation(latitude, longitude);
    }
}