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

    [StringLength(100)]
    public string Descricao { get; set; }


    public override string ToString()
    {
        if (UnidadeMedida is UnidadeMedida.Un)
        {
            return $"{Nome} {Descricao}";
        }
        return $"{Nome} {Descricao} {Quantidade} {UnidadeMedida}";
    }
}