using ListaDeComprasInteligente.Shared.Models.Request;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.ComponentModel.DataAnnotations;

namespace ListaDeComprasInteligente.WebApp.Components;

public partial class FormularioBuscaBase : ComponentBase
{
    [Parameter] public EventCallback<ListaComprasRequest> OnValidSubmit { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }

	protected ListaComprasRequest Model = new() { Produtos = Array.Empty<ProdutoRequest>() };
    protected Array UnidadesMedida = Enum.GetValues(typeof(UnidadeMedida));

	protected override void OnInitialized()
	{
		AddProduct();
		base.OnInitialized();
	}


	protected void AddProduct() =>
		Model.Produtos = Model.Produtos.ToList().Prepend(BuildDefaultProduct());

	protected void RemoveLastProduct(ProdutoRequest produto) =>
		Model.Produtos = Model.Produtos.Where(p => p != produto);


	protected void Submit()
    {
		if (IsFormValid())
		{
			OnValidSubmit.InvokeAsync(Model);
			return;
		}
		
		Snackbar.Add("Parâmetros inválidos", Severity.Warning);
    }


    #region Builders
    private static ProdutoRequest BuildDefaultProduct() => new()
	{
		Nome = string.Empty,
		Quantidade = 1,
		UnidadeMedida = UnidadeMedida.Un
	};
    #endregion


    #region Validations
	private bool IsFormValid()
	{
		var validationResults = new List<ValidationResult>();

		var isValidForm = Model.Produtos.All(p =>
		{
			var validationContext = new ValidationContext(p);
			var isValidModel = Validator.TryValidateObject(p, validationContext, validationResults, true);
			return isValidModel;
		});
		return isValidForm;
	}
    #endregion
}
