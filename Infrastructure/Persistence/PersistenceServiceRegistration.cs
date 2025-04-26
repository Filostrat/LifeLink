using Application.Contracts.Persistence;

using Domain.Settings;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;


namespace Persistence;

public static class PersistenceServiceRegistration
{
	public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<DonorDbContext>(options =>
			options.UseSqlServer(
				configuration.GetConnectionString("LifeLinkConnectionString"),
				b => b
					.MigrationsAssembly(typeof(DonorDbContext).Assembly.FullName)
					.UseNetTopologySuite() 
			));


		services.Configure<DonorSettings>(configuration.GetSection("DonorSettings"));

		services.AddTransient<IDonorRepository, DonorRepository>();
		services.AddTransient<IBloodTypeRepository, BloodTypeRepository>();
		services.AddTransient<IDonationRequestRepository, DonationRequestRepository>();

		return services;
	}

	public static void ApplyPersistenceMigrations(this IServiceProvider serviceProvider)
	{
		using var scope = serviceProvider.CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<DonorDbContext>();

		if (dbContext.Database.GetService<IRelationalDatabaseCreator>() is RelationalDatabaseCreator databaseCreator)
		{
			if (!databaseCreator.Exists())
			{
				dbContext.Database.Migrate();
			}
			else
			{
				var pendingMigrations = dbContext.Database.GetPendingMigrations();
				if (pendingMigrations.Any())
				{
					dbContext.Database.Migrate();
				}
			}
		}
	}
}