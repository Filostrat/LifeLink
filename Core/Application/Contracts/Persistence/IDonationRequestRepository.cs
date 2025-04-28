using Domain;


namespace Application.Contracts.Persistence;

public interface IDonationRequestRepository : IGenericRepository<DonationRequest>
{
	Task<List<DonationRequest>> GetAllWithNotificationsAsync(CancellationToken cancellationToken, string adminId = null);
	Task<DonationRequest> GetWithIncludesAsync(int id, CancellationToken cancellationToken, string adminId = null);
}