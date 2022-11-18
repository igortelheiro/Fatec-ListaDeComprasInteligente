using ListaDeComprasInteligente.Domain.ValueObjects;
using ListaDeComprasInteligente.Shared.Models.Request;
using Serilog;

namespace ListaDeComprasInteligente.Domain;

public class ListaCompras
{
    public ListaComprasRequest ParametrosBusca { get; }
    public IDictionary<string, List<Anuncio>> Produtos { get; }
    public List<Fornecedor> Fornecedores { get; set; }
    public Fornecedor? FornecedorMaisCompetitivo { get; set; }

    private static readonly string[] _mercadosPrioritarios = new[] { "Carrefour", "Clube Extra", "Pão de Açúcar" };

    public ListaCompras(ListaComprasRequest listaComprasRequest)
    {
        ParametrosBusca = listaComprasRequest;
        Produtos = new Dictionary<string, List<Anuncio>>();
        Fornecedores = new();
    }


    public void AdicionarProduto(string nomeProduto, List<Anuncio> anuncios)
    {
        var produtoExistente = Produtos.ContainsKey(nomeProduto);
        if (produtoExistente)
        {
            Log.Warning("Produto {nome} já existente na lista de compras", nomeProduto);
            return;
        }

        Produtos.Add(nomeProduto, anuncios);
    }


    // TODO: tirar média dos valores e excluir grandes disparidades
    public void OrganizarProdutosPorFornecedor()
    {
        foreach (var produto in Produtos)
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
                var produtoRequest = ParametrosBusca.Produtos.First(p => p.Nome == produto.Key);
                if (produtoRequest is null) continue;

                var produtoResponse = new Produto(anuncio.Titulo, produtoRequest.Quantidade, anuncio.Preco);

                var fornecedorExistente = Fornecedores.FirstOrDefault(f => f.Nome == anuncio.NomeFornecedor);
                if (fornecedorExistente is not null)
                {
                    fornecedorExistente.AdicionarProduto(produtoResponse);
                    continue;
                }

                var newFornecedor = new Fornecedor(anuncio.NomeFornecedor);
                newFornecedor.AdicionarProduto(produtoResponse);
                Fornecedores.Add(newFornecedor);
            }
        }
    }


    public void EncontrarFornecedorMaisCompetitivo()
    {
        var fornecedoresCompletos = Fornecedores.FindAll(f => f.Produtos.Count() == ParametrosBusca.Produtos.Count());

        FornecedorMaisCompetitivo = fornecedoresCompletos.Any()
                                  ? fornecedoresCompletos.MinBy(f => f.PrecoTotal)!
                                  : Fornecedores.MinBy(f => f.PrecoTotal)!;

        Fornecedores.Remove(FornecedorMaisCompetitivo);
    }

}