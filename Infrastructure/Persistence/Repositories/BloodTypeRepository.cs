using Application.Contracts.Persistence;

using Domain;


namespace Persistence.Repositories;

public class BloodTypeRepository : GenericRepository<BloodType>, IBloodTypeRepository
{
	private readonly DonorDbContext _dbContext;

	public BloodTypeRepository(DonorDbContext dbContext) : base(dbContext)
	{
		_dbContext = dbContext;
	}
}