using ListaDeComprasInteligente.Domain.Exceptions;

namespace ListaDeComprasInteligente.Domain;

public class ListaCompras
{
    public IDictionary<string, IEnumerable<Anuncio>> Produtos { get; }

    public ListaCompras()
    {
        Produtos = new Dictionary<string, IEnumerable<Anuncio>>();
    }


    // TODO: tirar média dos valores e excluir grandes disparidades
    public void AdicionarProduto(string nomeProduto)
    {
        var produtoExistente = Produtos.ContainsKey(nomeProduto);
        if (produtoExistente)
        {
            throw new DomainException("Produto já existente na lista de compras");
        }

        Produtos.Add(nomeProduto, Array.Empty<Anuncio>());
    }


    public void AdicionarAnuncio(string nomeProduto, Anuncio novoAnuncio)
    {
        var produtoNaLista = Produtos.TryGetValue(nomeProduto, out var anunciosExistentes);
        if (produtoNaLista is false)
        {
            throw new DomainException("Impossível adicionar anúncio. Produto não existente na lista de compras");
        }

        Produtos[nomeProduto] = anunciosExistentes!.ToList().Prepend(novoAnuncio);

        RemoverAnuncioMenosCompetitivoFornecedor(nomeProduto, anunciosExistentes!, novoAnuncio);
    }


    private void RemoverAnuncioMenosCompetitivoFornecedor(string nomeProduto, IEnumerable<Anuncio> anunciosExistentes, Anuncio novoAnuncio)
    {
        var anuncioFornecedorExistente = anunciosExistentes!.FirstOrDefault(a => a.NomeFornecedor == novoAnuncio.NomeFornecedor);
        if (anuncioFornecedorExistente is not null && novoAnuncio.Preco < anuncioFornecedorExistente.Preco)
        {
            Produtos[nomeProduto] = anunciosExistentes!.ToList().Where(a => a != anuncioFornecedorExistente);
        }
    }
}