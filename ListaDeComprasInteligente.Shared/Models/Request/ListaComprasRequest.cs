using System.ComponentModel.DataAnnotations;

namespace ListaDeComprasInteligente.Shared.Models.Request;

public class ListaComprasRequest
{
    [Required]
    [MinLength(1)]
    public IEnumerable<ProdutoRequest> Produtos { get; set; }
    // TODO: Adicionar busca por categoria
}