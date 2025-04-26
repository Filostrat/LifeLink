using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


using Persistence;

public class DonorDbContextFactory : IDesignTimeDbContextFactory<DonorDbContext>
{
	public DonorDbContext CreateDbContext(string[] args)
	{
		IConfigurationRoot configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json")
			.Build();

		var builder = new DbContextOptionsBuilder<DonorDbContext>();
		var connectionString = configuration.GetConnectionString("DonorConnectionString");

		builder.UseSqlServer(connectionString, x => x.UseNetTopologySuite());

		return new DonorDbContext(builder.Options);
	}
}