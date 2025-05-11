using Application.Contracts.Infrastructure;
using Application.Contracts.Notifications;
using Application.DTOs.Notifications;
using Domain;
using Domain.Settings;

using Kafka.Base.Interfaces;
using Kafka.Factories.Interfaces;

using Microsoft.Extensions.Options;

using Newtonsoft.Json;


namespace Infrastructure.NotificationChannelServices;

public class EmailChannelService : IDonationRequestNotificationChannelService
{
	public NotificationChannelEnum ChannelName => NotificationChannelEnum.Email;

	private readonly IEmailTemplateBuilder _emailTemplateBuilder;
	private readonly IBaseProducer _producer;
	private readonly string _topic;

	public EmailChannelService(IKafkaFactory kafkaFactory,
							   IEmailTemplateBuilder emailTemplateBuilder,	
							   IOptions<KafkaSettings> settings)
	{
		_producer = kafkaFactory.GetProducer("EmailProducer");
		_topic = settings.Value.TopicEmail;
		_emailTemplateBuilder = emailTemplateBuilder;
	}

	public async Task PublishAsync(DonationNotificationInfoDTO message, CancellationToken ct)
	{
		var emailDto = await _emailTemplateBuilder
			.CreateDonationRequestEmail(
				message.Email,
				message.City,
				message.Latitude,
				message.Longitude);


		await _producer.SendAsync(_topic, JsonConvert.SerializeObject(emailDto), ct);
	}
}