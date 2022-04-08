using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ListaDeComprasInteligente.Service.Models.Request;

public class QuantidadeRequest
{
    [Required] public int ValorQuantidade { get; set; }
    [Required] public decimal Medida { get; set; }
    
    [EnumDataType(typeof(UnidadeMedida))]
    [JsonConverter(typeof(StringEnumConverter))]
    [Required] public UnidadeMedida UnidadeMedida { get; set; }
    

    public override string ToString()
    {
        if (UnidadeMedida is UnidadeMedida.Unidade)
        {
            return string.Empty;
        }
        return $"{Medida} {UnidadeMedidaToString(UnidadeMedida)}";
    }

    
    private static string UnidadeMedidaToString(UnidadeMedida unidadeMedida) =>
        unidadeMedida switch
        {
            UnidadeMedida.Unidade => string.Empty,
            UnidadeMedida.Grama => "g",
            UnidadeMedida.Miligrama => "mg",
            UnidadeMedida.Kilograma => "Kg",
            UnidadeMedida.Mililitro => "ml",
            UnidadeMedida.Litro => "L",
            UnidadeMedida.Metro => "m",
            _ => throw new ArgumentOutOfRangeException(nameof(unidadeMedida), unidadeMedida, null)
        };
}


public enum UnidadeMedida
{
    Unidade,
    Grama,
    Miligrama,
    Kilograma,
    Mililitro,
    Litro,
    Metro
}