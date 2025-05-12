using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Kafka.Base.Interfaces;
using TelegramBot.Data;
using Kafka.Factories.Interfaces;

public class TelegramConsumerHostedService : BackgroundService
{
	private readonly IBaseConsumer _consumer;
	private readonly ITelegramBotClient _botClient;
	private readonly TelegramDbContext _dbContext;
	private readonly IOptions<KafkaSettings> _kafkaSettings;
	private readonly ILogger<TelegramConsumerHostedService> _logger;

	public TelegramConsumerHostedService(
		IKafkaFactory kafkaFactory,
		ITelegramBotClient botClient,
		TelegramDbContext dbContext,
		IOptions<KafkaSettings> kafkaSettings,
		ILogger<TelegramConsumerHostedService> logger)
	{
		_consumer = kafkaFactory.GetConsumer("TelegramConsumer");
		_botClient = botClient;
		_dbContext = dbContext;
		_kafkaSettings = kafkaSettings;
		_logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		var topic = _kafkaSettings.Value.Topic;
		_consumer.Subscribe(topic);
		_logger.LogInformation("TelegramConsumer subscribed to topic {Topic}", topic);

		while (!stoppingToken.IsCancellationRequested)
		{
			try
			{
				var msg = await _consumer.ConsumeAsync(stoppingToken);
				if (msg?.Message?.Value == null)
					continue;

				var payload = JsonConvert.DeserializeObject<TelegramMessage>(msg.Message.Value);
				if (payload == null)
					continue;

				var sub = await _dbContext.Subscriptions
					.SingleOrDefaultAsync(s => s.Email == payload.Email, stoppingToken);

				if (sub == null)
				{
					_logger.LogWarning("Не знайдено підписку для {Email}", payload.Email);
					continue;
				}

				await _botClient.SendMessage(
					chatId: sub.ChatId,
					text: payload.Text,
					cancellationToken: stoppingToken
				);

				_logger.LogInformation(
					"Sent to {Email} (chat {ChatId}): {Text}",
					payload.Email, sub.ChatId, payload.Text);
			}
			catch (OperationCanceledException) { }
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error in TelegramConsumerHostedService");
			}
		}
	}

	private class TelegramMessage
	{
		public string Email { get; set; } = null!;
		public string Text { get; set; } = null!;
	}
}
