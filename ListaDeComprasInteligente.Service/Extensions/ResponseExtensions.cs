namespace ListaDeComprasInteligente.Service.Extensions;

public static class ResponseExtensions
{
    public static string ToVisualPrice(this decimal price) => $"R${price:0.00}";
}