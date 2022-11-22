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
            Log.Warning("Produto {nomeProduto} já existente na lista de compras", nomeProduto);
            return;
        }

        Produtos.Add(nomeProduto, anuncios);
    }


    public void OrganizarProdutosPorFornecedor()
    {
        foreach (var produto in Produtos)
        {
            var anuncios = produto.Value;
            if (anuncios is null) continue;

            OrganizarAnunciosPorFornecedor(ref anuncios);
            AdicionarProdutoAoFornecedor(produto.Key, ref anuncios);

        }

        FiltrarFornecedoresCompletos();
        FiltrarTop3Fornecedores();
    }


    #region Private
    public void EncontrarFornecedorMaisCompetitivo()
    {
        if (Fornecedores.Count is 0) return;

        FornecedorMaisCompetitivo = Fornecedores.MinBy(_=>_.PrecoTotal)!;
        Fornecedores.Remove(FornecedorMaisCompetitivo);
    }


    private void OrganizarAnunciosPorFornecedor(ref List<Anuncio> anuncios)
    {
        anuncios = anuncios.DistinctBy(_=>_.NomeFornecedor).ToList();
        if (anuncios.Count is 0)
        {
            Fornecedores.Add(new("Não encontrado"));
            return;
        }
    }


    private void AdicionarProdutoAoFornecedor(string nomeProduto, ref List<Anuncio> anuncios)
    {
        foreach (var anuncio in anuncios)
        {
            var request = ParametrosBusca.Produtos.FirstOrDefault(_=>_.Nome == nomeProduto);
            if (request is null) continue;

            var newProduto = new Produto(anuncio.Titulo, request.Quantidade, anuncio.Preco);

            var fornecedorExistente = Fornecedores.FirstOrDefault(_=>_.Nome == anuncio.NomeFornecedor);
            if (fornecedorExistente is not null)
            {
                fornecedorExistente.AdicionarProduto(newProduto);
                continue;
            }

            var newFornecedor = new Fornecedor(anuncio.NomeFornecedor);
            newFornecedor.AdicionarProduto(newProduto);
            Fornecedores.Add(newFornecedor);
        }
    }


    private void FiltrarFornecedoresCompletos()
    {
        var fornecedoresCompletos = Fornecedores
            .FindAll(_ => _.Produtos.Count == ParametrosBusca.Produtos.Count())
            .Select(_ => _.Nome);

        Fornecedores = Fornecedores.Where(_ => fornecedoresCompletos.Contains(_.Nome)).ToList();
    }


    private void FiltrarTop3Fornecedores() =>
        Fornecedores = Fornecedores.OrderBy(_=>_.PrecoTotal).Take(3).ToList();
    #endregion
}