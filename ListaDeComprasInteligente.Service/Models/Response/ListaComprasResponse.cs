using ListaDeComprasInteligente.Domain;
using ListaDeComprasInteligente.Service.Models.Request;

namespace ListaDeComprasInteligente.Service.Models.Response;

public class ListaComprasResponse
{
    public List<FornecedorResponse> Fornecedores { get; }
    public FornecedorResponse FornecedorMaisCompetitivo { get; private set; }
    
    public ListaComprasResponse(ListaCompras listaCompras, ListaComprasRequest request)
    {
        Fornecedores = new List<FornecedorResponse>();

        var mercadosPrioritarios = new string[] { "Carrefour", "Clube Extra", "Pão de Açúcar" };
        
        foreach (var produto in listaCompras.Produtos)
        {
            var disponibilidade = produto.Disponibilidade?.DistinctBy(d => d.NomeFornecedor)?.Select(d => d)?.ToList();
            if (disponibilidade?.Count() is 0)
            {
                return;
            }
            
            var top3Disponibilidades = new List<Disponibilidade>();
            for (int i = 0; i < 3; i++)
            {
                var melhorOpcao = disponibilidade?.FirstOrDefault(d => mercadosPrioritarios.Contains(d.NomeFornecedor, StringComparer.InvariantCultureIgnoreCase));
                if (melhorOpcao is null)
                {
                    melhorOpcao = disponibilidade?.MinBy(d => d.Preco);
                }
                top3Disponibilidades.Add(melhorOpcao);
                disponibilidade?.Remove(melhorOpcao);
            }
            
            foreach (var fornecedor in top3Disponibilidades)
            {
                if (fornecedor is null)
                {
                    continue;
                }
                var produtoRequest = request.Produtos.First(p => p.Nome == produto.Nome);
                if (produtoRequest is null)
                {
                    continue;
                }
                var produtoResponse = new ProdutoResponse(fornecedor.TituloAnuncio, produtoRequest.Quantidade.ValorQuantidade, fornecedor.Preco);
                
                var fornecedorExistente = Fornecedores.FirstOrDefault(f => f.Nome == fornecedor.NomeFornecedor);
                if (fornecedorExistente is not null)
                {
                    fornecedorExistente.AdicionarProduto(produtoResponse);
                    continue;
                }
                
                var newFornecedor = new FornecedorResponse(fornecedor.NomeFornecedor);
                newFornecedor.AdicionarProduto(produtoResponse);
                Fornecedores.Add(newFornecedor);
            }
        }

        EncontrarFornecedorMaisCompetitivo(request);
    }


    // TODO: melhorar lógica e favorecer fornecedores com todos os produtos
    private void EncontrarFornecedorMaisCompetitivo(ListaComprasRequest request)
    {
        var quantidadeProdutosRequest = request.Produtos.Count();
        var fornecedoresCompletos = Fornecedores.FindAll(f => f.Produtos.Count() == quantidadeProdutosRequest);
        
        FornecedorMaisCompetitivo = fornecedoresCompletos.Any()
                                  ? fornecedoresCompletos.MinBy(f => f.PrecoTotal)
                                  : Fornecedores.MinBy(f => f.PrecoTotal);
        
        Fornecedores.Remove(FornecedorMaisCompetitivo);
    }
}