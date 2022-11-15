namespace ListaDeComprasInteligente.Shared.Models.Response;

public record FornecedorResponse(string Nome, IEnumerable<ProdutoResponse> Produtos, string PrecoTotal);