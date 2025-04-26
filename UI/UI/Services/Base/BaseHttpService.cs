using System.Net.Http.Headers;

using UI.Contracts;

namespace UI.Services.Base;

public class BaseHttpService
{
	protected readonly ILocalStorageService _localStorage;

	protected IClient _client;

	public BaseHttpService(IClient client, ILocalStorageService localStorage)
	{
		_client = client;
		_localStorage = localStorage;

	}

	protected Response<Guid> ConvertApiExceptions<Guid>(ApiException ex)
	{
		if (ex.StatusCode == 400)
		{
			return new Response<Guid>() { Message = "Validation errors have occurred.", ValidationErrors = ex.Response, Success = false };
		}
		else if (ex.StatusCode == 404)
		{
			return new Response<Guid>() { Message = "The requested item could not be found.", Success = false };
		}
		else
		{
			return new Response<Guid>() { Message = "Something went wrong, please try again.", Success = false };
		}
	}

	protected void AddBearerToken()
	{
		var token = _localStorage.GetStorageValue<string>("token");
		if (!string.IsNullOrEmpty(token))
		{
			_client.HttpClient.DefaultRequestHeaders.Authorization =
				new AuthenticationHeaderValue("Bearer", token);
		}
	}
}