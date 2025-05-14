using Application.DTOs.Notifications;

using Domain.Settings;

using Kafka.Base.Interfaces;
using Kafka.Factories.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Data;



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
		var topic = _kafkaSettings.Value.TopicTelegram;
		_consumer.Subscribe(topic);
		_logger.LogInformation("TelegramConsumer subscribed to topic {Topic}", topic);

		while (!stoppingToken.IsCancellationRequested)
		{
			try
			{
				var msg = await _consumer.ConsumeAsync(stoppingToken);
				if (msg?.Message?.Value == null) continue;

				var payload = JsonConvert.DeserializeObject<DonationNotificationMessageInfo>(msg.Message.Value);
				if (payload == null) continue;

				var sub = await _dbContext.Subscriptions
					.SingleOrDefaultAsync(s => s.Email == payload.Email, stoppingToken);

				if (sub == null)
				{
					_logger.LogWarning("Не знайдено підписку для {Email}", payload.Email);
					continue;
				}

				var text = $@"<b>Потрібна твоя допомога</b>

				В місті <b>{payload.City}</b> потрібна саме твоя кров!

				{payload.Message}

				Якщо ви більше не бажаєте отримувати такі повідомлення, просто проігноруйте це.";

				var inlineKeyboard = new InlineKeyboardMarkup(new[]
				{
				InlineKeyboardButton.WithUrl(
					text: "Переглянути локацію",
					url: $"https://www.google.com/maps/search/?api=1&query={payload.Latitude},{payload.Longitude}"
				)
			});

				await _botClient.SendMessage(
					chatId: sub.ChatId,
					text: text,
					parseMode: ParseMode.Html,
					replyMarkup: inlineKeyboard,
					cancellationToken: stoppingToken
				);

				_logger.LogInformation(
					"Sent to {Email} (chat {ChatId}): Donation alert for {City}",
					payload.Email, sub.ChatId, payload.City);
			}
			catch (OperationCanceledException) { }
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error in TelegramConsumerHostedService");
			}
		}
	}
}
