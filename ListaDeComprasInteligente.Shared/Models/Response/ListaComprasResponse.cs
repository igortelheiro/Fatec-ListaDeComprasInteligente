using ListaDeComprasInteligente.Domain;
using ListaDeComprasInteligente.Shared.Models.Request;

namespace ListaDeComprasInteligente.Shared.Models.Response;

public class ListaComprasResponse
{
    public IEnumerable<FornecedorResponse> Fornecedores { get; set; }
    public FornecedorResponse FornecedorMaisCompetitivo { get; set; }

    private static readonly string[] _mercadosPrioritarios = new[] { "Carrefour", "Clube Extra", "Pão de Açúcar" };

    public ListaComprasResponse()
    {
    }


    public ListaComprasResponse(ListaCompras listaCompras, ListaComprasRequest request)
    {
        Fornecedores = Array.Empty<FornecedorResponse>();
        FornecedorMaisCompetitivo = new(); 
        
        foreach (var produto in listaCompras.Produtos)
        {
            var anuncios = produto.Value?.DistinctBy(d => d.NomeFornecedor)?.ToList();
            if (anuncios?.Count is 0)
            {
                return;
            }
            
            var top3Anuncios = new List<Anuncio>();
            for (var i = 0; i < 3; i++)
            {
                var melhorAnuncio = anuncios!.FirstOrDefault(a => _mercadosPrioritarios.Contains(a.NomeFornecedor, StringComparer.InvariantCultureIgnoreCase))
                                 ?? anuncios!.MinBy(d => d.Preco);

                if (melhorAnuncio is null) continue;
                
                top3Anuncios.Add(melhorAnuncio);
                anuncios!.Remove(melhorAnuncio);
            }
            
            foreach (var anuncio in top3Anuncios)
            {
                var produtoRequest = request.Produtos.First(p => p.Nome == produto.Key);
                if (produtoRequest is null) continue;
                
                var produtoResponse = new ProdutoResponse(anuncio.Titulo, produtoRequest.Quantidade, anuncio.Preco);
                
                var fornecedorExistente = Fornecedores.FirstOrDefault(f => f.Nome == anuncio.NomeFornecedor);
                if (fornecedorExistente is not null)
                {
                    fornecedorExistente.AdicionarProduto(produtoResponse);
                    continue;
                }
                
                var newFornecedor = new FornecedorResponse(anuncio.NomeFornecedor);
                newFornecedor.AdicionarProduto(produtoResponse);
                Fornecedores = Fornecedores.Concat(new FornecedorResponse[] { newFornecedor });
            }
        }

        EncontrarFornecedorMaisCompetitivo(request);
    }


    private void EncontrarFornecedorMaisCompetitivo(ListaComprasRequest request)
    {
        var fornecedoresCompletos = Fornecedores.ToList()
                                                .FindAll(f => f.Produtos.Count() == request.Produtos.Count());
        
        FornecedorMaisCompetitivo = fornecedoresCompletos.Any()
                                    ? fornecedoresCompletos.MinBy(f => f.PrecoTotal)!
                                    : Fornecedores.MinBy(f => f.PrecoTotal)!;
        
        Fornecedores = Fornecedores.Where(f => f != FornecedorMaisCompetitivo);
    }
}