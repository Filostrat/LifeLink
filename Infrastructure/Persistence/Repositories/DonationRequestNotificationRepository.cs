using Application.Contracts.Persistence;

using Domain;


namespace Persistence.Repositories;

public class DonationRequestNotificationRepository : GenericRepository<DonationRequestNotification>, IDonationRequestNotificationRepository
{
	public DonationRequestNotificationRepository(DonorDbContext dbContext) : base(dbContext)
	{
	}
}
