using System.ComponentModel.DataAnnotations;

namespace ListaDeComprasInteligente.Service.Models.Request;

public class ProdutoRequest
{
    [Required] public string Nome { get; set; }
    public QuantidadeRequest? Quantidade { get; set; }
    public string? Detalhes { get; set; }
}

