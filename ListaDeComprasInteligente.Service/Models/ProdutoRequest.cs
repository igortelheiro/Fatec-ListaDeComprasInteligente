using System.ComponentModel.DataAnnotations;

namespace ListaDeComprasInteligente.Service.Models;

public class ProdutoRequest
{
    [Required]
    public string PalavrasChave { get; set; }
}