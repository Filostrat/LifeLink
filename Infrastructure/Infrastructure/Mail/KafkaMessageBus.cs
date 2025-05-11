using Application.Contracts.Infrastructure;
using Domain.Settings;
using Kafka.Base.Interfaces;
using Kafka.Factories.Interfaces;

using Microsoft.Extensions.Options;

using Newtonsoft.Json;


namespace Infrastructure.Mail;

public class KafkaMessageBus : IMessageBus
{
	private readonly IBaseProducer _producer;
	private readonly IOptions<KafkaSettings> _kafkaSettings;

	public KafkaMessageBus(IKafkaFactory kafkaFactory, IOptions<KafkaSettings> kafkaSettings)
	{
		_producer = kafkaFactory.GetProducer("EmailProducer");
		_kafkaSettings = kafkaSettings;
	}

	public async Task<bool> PublishAsync<T>(T message, CancellationToken cancellationToken)
	{
		return await _producer.SendAsync(_kafkaSettings.Value.Topic, JsonConvert.SerializeObject(message), cancellationToken);
	}
}