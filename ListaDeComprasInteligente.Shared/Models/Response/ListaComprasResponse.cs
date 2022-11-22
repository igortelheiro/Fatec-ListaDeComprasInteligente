using ListaDeComprasInteligente.Shared.Models.Request;

namespace ListaDeComprasInteligente.Shared.Models.Response;

public record ListaComprasResponse(IEnumerable<FornecedorResponse> Fornecedores,
                                   FornecedorResponse? FornecedorMaisCompetitivo);