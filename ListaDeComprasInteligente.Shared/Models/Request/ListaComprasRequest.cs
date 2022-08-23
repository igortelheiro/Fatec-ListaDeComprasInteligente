using System.ComponentModel.DataAnnotations;

namespace ListaDeComprasInteligente.Shared.Models.Request;

public class ListaComprasRequest
{
    [Required]
    [MinLength(1)]
    public List<ProdutoRequest> Produtos { get; set; }
    // TODO: Adicionar busca por categoria
}