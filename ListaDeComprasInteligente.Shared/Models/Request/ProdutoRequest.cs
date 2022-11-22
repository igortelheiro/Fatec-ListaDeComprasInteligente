using Newtonsoft.Json;
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


    public override string ToString() => Nome;
}