using Application.Contracts.Notifications;

using Domain;
using Domain.Settings;

using Kafka.Base.Interfaces;
using Kafka.Factories.Interfaces;

using Microsoft.Extensions.Options;

using Newtonsoft.Json;


namespace Infrastructure.NotificationChannelServices;

public class EmailChannelService : INotificationChannelService
{
	public NotificationChannelEnum ChannelName => NotificationChannelEnum.Email;

	private readonly IBaseProducer _producer;
	private readonly string _topic;

	public EmailChannelService(IKafkaFactory kafkaFactory,
							   IOptions<KafkaSettings> settings)
	{
		_producer = kafkaFactory.GetProducer("EmailProducer");
		_topic = settings.Value.TopicEmail;
	}

	public Task PublishAsync<T>(T message, CancellationToken ct) =>
		_producer.SendAsync(_topic, JsonConvert.SerializeObject(message), ct);
}