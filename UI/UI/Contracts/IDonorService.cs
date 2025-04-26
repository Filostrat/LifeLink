using UI.Models.Donors;

namespace UI.Contracts;

public interface IDonorService
{
	public Task<DonorVM> DonorInformation();
	public Task<DonorVM> UpdateDonorInformation(DonorVM donorVM);
}
