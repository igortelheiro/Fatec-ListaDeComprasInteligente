namespace ListaDeComprasInteligente.Domain;

public class Produto
{
    public string Nome { get; }
    public List<Disponibilidade> Disponibilidade { get; }
    
    public Produto(string nome)
    {
        Nome = nome;
        Disponibilidade = new List<Disponibilidade>();
    }

    public void AdicionarDisponibilidade(Disponibilidade disponibilidade)
    {
        var fornecedorExistente = Disponibilidade.FirstOrDefault(d => d.NomeFornecedor == disponibilidade.NomeFornecedor);
        if (fornecedorExistente is null)
        {
            Disponibilidade.Add(disponibilidade);
            return;
        }

        if (disponibilidade.Preco >= fornecedorExistente.Preco)
        {
            return;
        }
        
        Disponibilidade.Add(disponibilidade);
        Disponibilidade.Remove(fornecedorExistente);
    }
}