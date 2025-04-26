using Application.Contracts.Persistence;

using Domain;


namespace Persistence.Repositories;
public class DonationRequestRepository : GenericRepository<DonationRequest>, IDonationRequestRepositoryRepository
{
	private readonly DonorDbContext _dbContext;

	public DonationRequestRepository(DonorDbContext dbContext) : base(dbContext)
	{
		_dbContext = dbContext;
	}
}
