using Microsoft.EntityFrameworkCore;

using System.Text;

using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Data;
using TelegramBot.Models;



public class TelegramBotWorker : BackgroundService
{
	private readonly ILogger<TelegramBotWorker> _logger;
	private readonly ITelegramBotClient _botClient;
	private readonly IServiceScopeFactory _scopeFactory;

	public TelegramBotWorker(
		ILogger<TelegramBotWorker> logger,
		ITelegramBotClient botClient,
		IServiceScopeFactory scopeFactory)
	{
		_logger = logger;
		_botClient = botClient;
		_scopeFactory = scopeFactory;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		var receiverOptions = new ReceiverOptions
		{
			AllowedUpdates = [UpdateType.Message]
		};
		var updateHandler = new DefaultUpdateHandler(
			HandleUpdateAsync,
			HandleErrorAsync
		);

		_botClient.StartReceiving(
			updateHandler: updateHandler,
			receiverOptions: receiverOptions,
			cancellationToken: stoppingToken
		);

		_logger.LogInformation("TelegramBotWorker has been launched. We are waiting for updates...");
		await Task.Delay(Timeout.Infinite, stoppingToken);
	}

	private async Task HandleUpdateAsync(
		ITelegramBotClient botClient,
		Update update,
		CancellationToken ct)
	{
		if (update.Message?.Type != MessageType.Text)
			return;

		var text = update.Message.Text!.Trim();
		if (!text.StartsWith("/start "))
			return;

		string email;

		try
		{
			var payload = text["/start ".Length..];
			var bytes = Convert.FromBase64String(payload);
			email = Encoding.UTF8.GetString(bytes);
		}
		catch
		{
			_logger.LogWarning("Невірний payload у чаті {ChatId}", update.Message.Chat.Id);
			return;
		}

		using var scope = _scopeFactory.CreateScope();
		var db = scope.ServiceProvider.GetRequiredService<TelegramDbContext>();

		bool exists = await db.Subscriptions
			.AnyAsync(s => s.ChatId == update.Message.Chat.Id && s.Email == email, ct);

		if (!exists)
		{
			db.Subscriptions.Add(new TelegramSubscription
			{
				Email = email,
				ChatId = update.Message.Chat.Id,
				SubscribedAt = DateTime.UtcNow
			});
			await db.SaveChangesAsync(ct);
			_logger.LogInformation("Підписка збережена: {Email} → {ChatId}", email, update.Message.Chat.Id);
		}
	}

	private Task HandleErrorAsync(
		ITelegramBotClient botClient,
		Exception exception,
		CancellationToken ct)
	{
		_logger.LogError(exception, "Помилка polling-бота:");
		return Task.CompletedTask;
	}
}