namespace ListaDeComprasInteligente.Domain;

public class Produto
{
    public string Nome { get; }
    public IEnumerable<Disponibilidade> Disponibilidade { get; }
    
    public Produto(string nome, IEnumerable<Disponibilidade> disponibilidade)
    {
        Nome = nome;
        Disponibilidade = disponibilidade;
    }
}