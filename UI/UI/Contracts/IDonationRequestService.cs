using UI.Models.DonationRequest;

namespace UI.Contracts;

public interface IDonationRequestService
{
	Task CreateDonationRequest(CreateDonationRequestVM createDonationRequestVM);
	Task<List<DonationRequestVM>> GetAllDonationRequest();
	Task<DonationRequestVM> GetDonationRequestByIdAsync(int id);
}