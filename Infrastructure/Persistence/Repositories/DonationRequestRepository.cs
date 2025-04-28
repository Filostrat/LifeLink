using Application.Contracts.Persistence;

using Domain;
using Microsoft.EntityFrameworkCore;
using System.Threading;


namespace Persistence.Repositories;
public class DonationRequestRepository : GenericRepository<DonationRequest>, IDonationRequestRepository
{
	private readonly DonorDbContext _dbContext;

	public DonationRequestRepository(DonorDbContext dbContext) : base(dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<List<DonationRequest>> GetAllWithNotificationsAsync(CancellationToken cancellationToken,string adminId = null)
	{
		var query = _dbContext.Set<DonationRequest>()
			.Include(dr => dr.BloodType)
			.Include(dr => dr.Notifications)
			.AsQueryable();

		if (!string.IsNullOrEmpty(adminId))
			query = query.Where(dr => dr.AdminId == adminId);

		return await query.ToListAsync(cancellationToken);
	}


	public async Task<DonationRequest> GetWithIncludesAsync(int id, CancellationToken cancellationToken, string adminId = null)
	{
		var query = _dbContext.Set<DonationRequest>()
			.Include(dr => dr.BloodType)
			.Include(dr => dr.Notifications)
			.AsQueryable();

		if (!string.IsNullOrEmpty(adminId))
			query = query.Where(dr => dr.AdminId == adminId);

		return await query.FirstOrDefaultAsync(dr => dr.Id == id, cancellationToken: cancellationToken);
	}
}