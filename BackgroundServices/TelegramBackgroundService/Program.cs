using Confluent.Kafka;

using Domain.Settings;

using Kafka;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using Telegram.Bot;
using TelegramBackgroundService;
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

		var botToken = cfg["TelegramToken"]
					 ?? throw new InvalidOperationException("TelegramToken is missing");
		services.AddSingleton<ITelegramBotClient>(sp =>
			new TelegramBotClient(botToken));

		services.Configure<KafkaSettings>(cfg.GetSection("KafkaConfiguration"));

		services.AddKafkaConsumer("TelegramConsumer", sp =>
		{
			var ks = sp.GetRequiredService<IOptions<KafkaSettings>>().Value;
			return new ConsumerConfig
			{
				BootstrapServers = ks.Server,
				GroupId = ks.GroupId,
				AutoOffsetReset = AutoOffsetReset.Earliest,
				EnableAutoCommit = true,
				EnablePartitionEof = true
			};
		});

		services.AddHostedService<TelegramBotWorker>();
		services.AddHostedService<TelegramConsumerHostedService>();
	});

var host = builder.Build();

host.Services.ApplyTelegramMigrations();

await host.RunAsync();