using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace ListaDeComprasInteligente.Shared.Models.Request;

public class ProdutoRequest
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Nome { get; set; }

    [Required]
    [Range(1, 999)]
    public decimal Quantidade { get; set; }

    [Required]
    [EnumDataType(typeof(UnidadeMedida))]
    [JsonConverter(typeof(StringEnumConverter))]
    public UnidadeMedida UnidadeMedida { get; set; }

    public override string ToString()
    {
        if (UnidadeMedida is UnidadeMedida.Un)
        {
            return Nome;
        }
        return $"{Nome} {Quantidade} {UnidadeMedida}";
    }


    //private static string UnidadeMedidaToString(UnidadeMedida unidadeMedida) =>
    //    unidadeMedida switch
    //    {
    //        UnidadeMedida.Unidade => string.Empty,
    //        UnidadeMedida.Grama => "g",
    //        UnidadeMedida.Miligrama => "mg",
    //        UnidadeMedida.Kilograma => "Kg",
    //        UnidadeMedida.Mililitro => "ml",
    //        UnidadeMedida.Litro => "L",
    //        UnidadeMedida.Metro => "m",
    //        _ => throw new ArgumentOutOfRangeException(nameof(unidadeMedida), unidadeMedida, null)
    //    };
}

public enum UnidadeMedida
{
    Un,
    g,
    mg,
    Kg,
    ml,
    L,
    m
}