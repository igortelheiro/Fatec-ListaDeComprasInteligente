namespace ListaDeComprasInteligente.Shared.Models.Response;

public class ProdutoResponse
{
    public string Nome { get; }
    public decimal Quantidade { get; }
    public decimal PrecoUnitario { get; }

    public ProdutoResponse()
    {
    }
    
    public ProdutoResponse(string nome, decimal quantidade, decimal precoUnitario)
    {
        Nome = nome;
        Quantidade = quantidade;
        PrecoUnitario = precoUnitario;
    }
}