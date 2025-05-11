using Microsoft.EntityFrameworkCore;

using TelegramBot.Models;


namespace TelegramBot.Data;

public class TelegramDbContext : DbContext
{
	public DbSet<TelegramSubscription> Subscriptions { get; set; }

	public TelegramDbContext(DbContextOptions<TelegramDbContext> opts)
		: base(opts) { }
}