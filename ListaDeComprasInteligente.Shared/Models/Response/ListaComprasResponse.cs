using ListaDeComprasInteligente.Shared.Models.Request;

namespace ListaDeComprasInteligente.Shared.Models.Response;

public record ListaComprasResponse(ListaComprasRequest ParametrosBusca,
                                   IEnumerable<FornecedorResponse> Fornecedores,
                                   FornecedorResponse? FornecedorMaisCompetitivo);