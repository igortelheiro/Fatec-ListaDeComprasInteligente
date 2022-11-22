using System.ComponentModel.DataAnnotations;

namespace ListaDeComprasInteligente.Shared.Models.Request;

// TODO: Adicionar busca por categoria
public record ListaComprasRequest
{
    [Required]
    [MinLength(1)]
    public IEnumerable<ProdutoRequest> Produtos { get; init; }
}