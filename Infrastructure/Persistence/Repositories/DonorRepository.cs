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

	public async Task<IEnumerable<Donor>> GetDonorsByBloodTypeAndLocationAsync(int recipientBloodTypeId, Point location)
	{
		var now = DateTime.UtcNow;
		var menThreshold = now.AddMonths(-_settings.MenDonationIntervalMonths);
		var womenThreshold = now.AddMonths(-_settings.WomenDonationIntervalMonths);

		_logger.LogInformation("Searching donors for blood type {BloodTypeId} within {Radius} meters", recipientBloodTypeId);

		var donors = await _dbContext.Donors
			.Include(d => d.BloodType)
			.Where(d =>
				_dbContext.BloodCompatibilities.Any(c =>
					c.FromBloodTypeId == d.BloodTypeId &&
					c.ToBloodTypeId == recipientBloodTypeId)

				&& (d.Location == null
					|| d.Location.Distance(location) <= _settings.RadiusInMeters)

				&& (!d.Weight.HasValue
					|| d.Weight.Value >= _settings.MinWeightKg)

				&& (
					d.Gender == null
					|| (d.Gender == "Male"
						&& (!d.LastDonation.HasValue || d.LastDonation <= menThreshold))
					|| (d.Gender == "Female"
						&& (!d.LastDonation.HasValue || d.LastDonation <= womenThreshold))
				)
			)
			.ToListAsync();

		_logger.LogInformation("Found {Count} eligible donors", donors.Count);
		return donors;
	}


	public async Task<Donor> GetByEmailAsync(string email)
	{
		var donor = await _dbContext.Donors
			.Include(d => d.BloodType)
			.FirstOrDefaultAsync(d => d.Email == email);

		return donor;
	}
}