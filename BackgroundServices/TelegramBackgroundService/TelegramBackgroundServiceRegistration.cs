using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using TelegramBot.Data;

namespace TelegramBackgroundService;

public static class TelegramBackgroundServiceRegistration
{

	public static void ApplyTelegramMigrations(this IServiceProvider serviceProvider)
	{
		using var scope = serviceProvider.CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<TelegramDbContext>();

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
