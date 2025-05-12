using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Data;

namespace TelegramBackgroundService;

public class TelegramDbContextFactory : IDesignTimeDbContextFactory<TelegramDbContext>
{
	public TelegramDbContext CreateDbContext(string[] args)
	{
		IConfigurationRoot configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json")
			.Build();

		var builder = new DbContextOptionsBuilder<TelegramDbContext>();
		var connectionString = configuration.GetConnectionString("TelegramDBConnectionString");

		builder.UseSqlServer(connectionString);

		return new TelegramDbContext(builder.Options);
	}
}
