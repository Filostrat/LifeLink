using Application.Contracts.Persistence;

using Domain;
using Microsoft.EntityFrameworkCore;


namespace Persistence.Repositories;
public class DonationRequestRepository : GenericRepository<DonationRequest>, IDonationRequestRepository
{
	private readonly DonorDbContext _dbContext;

	public DonationRequestRepository(DonorDbContext dbContext) : base(dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<List<DonationRequest>> GetAllWithNotificationsAsync(string adminId = null)
	{
		var query = _dbContext.Set<DonationRequest>()
			.Include(dr => dr.BloodType)
			.Include(dr => dr.Notifications)
			.AsQueryable();

		if (!string.IsNullOrEmpty(adminId))
			query = query.Where(dr => dr.AdminId == adminId);

		return await query.ToListAsync();
	}


	public async Task<DonationRequest> GetWithIncludesAsync(int id, string adminId = null)
	{
		var query = _dbContext.Set<DonationRequest>()
			.Include(dr => dr.BloodType)
			.Include(dr => dr.Notifications)
			.AsQueryable();

		if (!string.IsNullOrEmpty(adminId))
			query = query.Where(dr => dr.AdminId == adminId);

		return await query.FirstOrDefaultAsync(dr => dr.Id == id);
	}
}