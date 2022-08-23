
using ListaDeComprasInteligente.Shared.Models.Request;
using ListaDeComprasInteligente.Shared.Models.Response;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RestSharp;

namespace ListaDeComprasInteligente.WebApp.Pages;

public partial class BuscadorBase : ComponentBase
{
	[Inject] private ISnackbar Snackbar { get; set; }
	[Inject] private HttpClient HttpClient { get; set; }

	protected bool IsLoading = false;
    protected ListaComprasResponse? ListaComprasResponse = null;


	protected void ClearResponse() => ListaComprasResponse = null;


	protected async Task Submit(ListaComprasRequest request)
	{
		try
		{
			IsLoading = true;
			ListaComprasResponse = await BuildListaComprasAsync(request);
			Snackbar.Add("Lista gerada", Severity.Success);
		}
		catch
		{
			// log
			Snackbar.Add("Erro ao buscar lista de compras", Severity.Error);
		}
		finally
		{
			IsLoading = false;
		}
	}


	private async Task<ListaComprasResponse> BuildListaComprasAsync(ListaComprasRequest requestModel)
	{
		using var client = new RestClient(HttpClient, new RestClientOptions
		{
			BaseUrl = new Uri("http://localhost:5223")
		});

		var request = new RestRequest("ListaCompras", Method.Post).AddJsonBody(requestModel);
		// TODO: fix serialization
		var response = await client.ExecuteAsync<ListaComprasResponse>(request);

		return response.Data ?? new();
	}
}
