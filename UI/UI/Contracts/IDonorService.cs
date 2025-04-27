using UI.Models.Donors;

namespace UI.Contracts;

public interface IDonorService
{
	Task<DonorVM> DonorInformation();
	Task<DonorVM> UpdateDonorInformation(DonorVM donorVM);
}
