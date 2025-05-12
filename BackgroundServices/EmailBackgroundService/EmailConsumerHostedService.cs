using Application.Contracts.Infrastructure;

using Domain;

using Domain.Settings;

using Kafka.Base.Interfaces;

using Kafka.Factories.Interfaces;

using Microsoft.Extensions.Options;

using Newtonsoft.Json;


namespace EmailBackgroundService;

public class EmailConsumerHostedService : BackgroundService
{
	private readonly IBaseConsumer _consumer;
	private readonly IEmailSender _sender;
	private readonly IOptions<KafkaSettings> _kafkaSettings;
	private readonly ILogger<EmailConsumerHostedService> _logger;

	public EmailConsumerHostedService(IKafkaFactory factory,
									  IEmailSender sender,
									  IOptions<KafkaSettings> kafkaSettings,
									  ILogger<EmailConsumerHostedService> logger)
	{
		_consumer = factory.GetConsumer("EmailConsumer");
		_sender = sender;
		_kafkaSettings = kafkaSettings;
		_logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		_consumer.Subscribe(_kafkaSettings.Value.TopicEmail);
		_logger.LogInformation("Subscribed to topic {Topic}", _kafkaSettings.Value.TopicEmail);

		while (!cancellationToken.IsCancellationRequested)
		{
			try
			{
				var msg = await _consumer.ConsumeAsync(cancellationToken);
				if (msg?.Message?.Value != null)
				{
					var email = JsonConvert.DeserializeObject<Email>(msg.Message.Value);

					await _sender.SendEmailAsync(email);
					_logger.LogInformation("Email sent to {To}", email.To);
				}
			}
			catch (OperationCanceledException) { }
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error consuming email");
			}
		}
	}
}
