using UI.Contracts;
using UI.Services.Base;


namespace UI.Services;

public class DonationRequestService : BaseHttpService, IDonationRequestService
{
	public DonationRequestService(IClient client, ILocalStorageService localStorage) : base(client, localStorage)
	{
	}
}