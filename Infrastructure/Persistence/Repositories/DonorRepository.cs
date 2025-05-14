using Application.Contracts.Persistence;

using Domain;
using Domain.Settings;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using NetTopologySuite.Geometries;


namespace Persistence.Repositories;

public class DonorRepository : GenericRepository<Donor>, IDonorRepository
{
	private readonly DonorDbContext _dbContext;
	private readonly DonorSettings _settings;
	private readonly ILogger<DonorRepository> _logger;

	public DonorRepository(DonorDbContext dbContext, IOptions<DonorSettings> options, ILogger<DonorRepository> logger) : base(dbContext)
	{
		_dbContext = dbContext;
		_settings = options.Value;
		_logger = logger;
	}

	public async Task<IEnumerable<Donor>> GetDonorsByBloodTypeAndLocationAsync(
	List<int>? recipientBloodTypeIds,
	Point? location,
	CancellationToken cancellationToken)
	{
		var now = DateTime.UtcNow;
		var menThreshold = now.AddMonths(-_settings.MenDonationIntervalMonths);
		var womenThreshold = now.AddMonths(-_settings.WomenDonationIntervalMonths);

		List<int>? compatibleDonorBloodTypeIds = null;
		if (recipientBloodTypeIds != null && recipientBloodTypeIds.Any())
		{
			compatibleDonorBloodTypeIds = await _dbContext.BloodCompatibilities
				.Where(c => recipientBloodTypeIds.Contains(c.ToBloodTypeId))
				.GroupBy(c => c.FromBloodTypeId)
				.Where(g => g.Select(x => x.ToBloodTypeId).Distinct().Count() == recipientBloodTypeIds.Distinct().Count())
				.Select(g => g.Key)
				.ToListAsync(cancellationToken);
		}

		var query = _dbContext.Donors
			.Include(d => d.BloodType)
			.Include(d => d.Preference)
				.ThenInclude(p => p.Channels)
			.AsQueryable();

		if (compatibleDonorBloodTypeIds != null)
		{
			query = query.Where(d => compatibleDonorBloodTypeIds.Contains(d.BloodTypeId));
		}

		query = query.Where(d =>
			(!d.Weight.HasValue || d.Weight.Value >= _settings.MinWeightKg) &&
			(d.Gender == null ||
				(d.Gender == "Male" && (!d.LastDonation.HasValue || d.LastDonation <= menThreshold)) ||
				(d.Gender == "Female" && (!d.LastDonation.HasValue || d.LastDonation <= womenThreshold)))
		);

		return await query.ToListAsync(cancellationToken);
	}


	public async Task<Donor> GetByEmailAsync(string email, CancellationToken cancellationToken)
	{
		var donor = await _dbContext.Donors
			.Include(d => d.BloodType)
			.Include(d => d.Preference)
							 .ThenInclude(p => p.Channels)
			.SingleOrDefaultAsync(d => d.Email == email, cancellationToken);

		return donor;
	}
}