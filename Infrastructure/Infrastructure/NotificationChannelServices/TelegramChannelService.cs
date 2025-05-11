using Application.Contracts.Notifications;
using Application.DTOs.Notifications;
using Domain;
using Domain.Settings;

using Kafka.Base.Interfaces;
using Kafka.Factories.Interfaces;

using Microsoft.Extensions.Options;

using Newtonsoft.Json;


namespace Infrastructure.NotificationChannelServices;

public class TelegramChannelService : IDonationRequestNotificationChannelService
{
	public NotificationChannelEnum ChannelName => NotificationChannelEnum.Telegram;

	private readonly IBaseProducer _producer;
	private readonly string _topic;

	public TelegramChannelService(IKafkaFactory kafkaFactory,
								  IOptions<KafkaSettings> settings)
	{
		_producer = kafkaFactory.GetProducer("TelegramProducer");
		_topic = settings.Value.TopicTelegram;
	}

	public Task PublishAsync(DonationNotificationInfoDTO message, CancellationToken ct) =>
		_producer.SendAsync(_topic, JsonConvert.SerializeObject(message), ct);
}