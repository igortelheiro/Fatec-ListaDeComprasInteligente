namespace ListaDeComprasInteligente.Service.Models.Response;

public class ProdutoResponse
{
    public string Nome { get; }
    public int Quantidade { get; }
    public decimal PrecoUnitario { get; }
    
    public ProdutoResponse(string nome, int quantidade, decimal precoUnitario)
    {
        Nome = nome;
        Quantidade = quantidade;
        PrecoUnitario = precoUnitario;
    }
}