using UI.Contracts;
using UI.Models.Authentication;
using UI.Services.Base;

namespace UI.Services;

public class BloodTypeService : BaseHttpService, IBloodTypeService
{
	private readonly IClient _client;

	public BloodTypeService(IClient client, ILocalStorageService localStorage) : base(client, localStorage)
	{
		_client = client;
	}

	public async Task<ICollection<BloodTypeDto>> GetBloodTypes()
	{
		return (await _client.Blood_typesAsync()).Result;
	}
}
