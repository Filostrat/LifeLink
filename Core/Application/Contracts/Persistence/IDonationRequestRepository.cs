using Domain;


namespace Application.Contracts.Persistence;

public interface IDonationRequestRepository : IGenericRepository<DonationRequest>
{
	Task<List<DonationRequest>> GetAllWithNotificationsAsync(string adminId = null);
	Task<DonationRequest> GetWithIncludesAsync(int id, string adminId = null);
}