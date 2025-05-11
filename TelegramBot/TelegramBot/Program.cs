using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using TelegramBot;
using TelegramBot.Data;


var builder = Host.CreateDefaultBuilder(args)
.ConfigureAppConfiguration((hostingCtx, config) =>
	{
		config.AddEnvironmentVariables();
	})
	.ConfigureServices((hostingCtx, services) =>
	{
		var cfg = hostingCtx.Configuration;

		var telegramConn = cfg.GetConnectionString("TelegramDBConnectionString");
		services.AddDbContext<TelegramDbContext>(opts =>
			opts.UseSqlServer(telegramConn));


		var botToken = cfg["TelegramToken"];
		services.AddSingleton<ITelegramBotClient>(sp =>
			new TelegramBotClient(botToken));

		services.AddHostedService<TelegramBotWorker>();
	});

await builder.RunConsoleAsync();
