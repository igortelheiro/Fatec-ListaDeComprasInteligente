using System.ComponentModel.DataAnnotations;

namespace ListaDeComprasInteligente.Service.Models;

public class ListaComprasRequest
{
    [Required]
    public IEnumerable<ProdutoRequest> Produtos { get; set; }
}