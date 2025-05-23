﻿using Domain;

using Microsoft.EntityFrameworkCore;

using Persistence.Configurations;


namespace Persistence;

public class DonorDbContext : DbContext
{
	public DonorDbContext(DbContextOptions<DonorDbContext> opts)
		: base(opts) { }

	public DbSet<Donor> Donors { get; set; }
	public DbSet<BloodType> BloodTypes { get; set; }
	public DbSet<BloodCompatibility> BloodCompatibilities { get; set; }
	public DbSet<DonationRequest> DonationRequests { get; set; }
	public DbSet<NotificationPreference> NotificationPreferences { get; set; }
	public DbSet<NotificationChannel> NotificationChannels { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfiguration(new DonorConfiguration());
		modelBuilder.ApplyConfiguration(new BloodTypeConfiguration());
		modelBuilder.ApplyConfiguration(new DonationRequestConfiguration());
		modelBuilder.ApplyConfiguration(new BloodCompatibilityConfiguration());
		modelBuilder.ApplyConfiguration(new NotificationPreferenceConfiguration());
		modelBuilder.ApplyConfiguration(new NotificationChannelConfiguration());
		modelBuilder.ApplyConfiguration(new DonationRequestNotificationConfiguration());
	}
}