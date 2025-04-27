using UI.Models.DonationRequest;

namespace UI.Contracts;

public interface IDonationRequestService
{
	public Task CreateDonationRequest(CreateDonationRequestVM createDonationRequestVM);
}