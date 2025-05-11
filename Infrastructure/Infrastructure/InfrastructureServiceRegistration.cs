using Kafka;

using Confluent.Kafka;

using Application.Contracts.Infrastructure;

using Infrastructure.Mail;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Domain.Settings;
using Application.Contracts.Notifications;
using Infrastructure.NotificationChannelServices;


namespace Infrastructure;

public static class InfrastructureServicesRegistration
{
	public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

		services.AddTransient<IEmailSender, EmailSender>();
		services.AddTransient<IMessageBus, KafkaMessageBus>();
		services.AddTransient<IEmailTemplateBuilder, EmailTemplateBuilder>();
		services.AddTransient<INotificationChannelService, EmailChannelService>();
		services.AddTransient<INotificationChannelService, TelegramChannelService>();
		services.AddKafkaFactory();
		services.AddHostedService<EmailConsumerHostedService>();

		services.Configure<KafkaSettings>(configuration.GetSection("KafkaConfiguration"));

		services.AddKafkaProducer("EmailProducer", sp =>
		{
			var kafkaOptions = sp.GetRequiredService<IOptions<KafkaSettings>>().Value;
			return new ProducerConfig
			{
				BootstrapServers = kafkaOptions.Server,
			};
		});

		services.AddKafkaProducer("TelegramProducer", sp =>
		{
			var kafkaOptions = sp.GetRequiredService<IOptions<KafkaSettings>>().Value;
			return new ProducerConfig
			{
				BootstrapServers = kafkaOptions.Server
			};
		});

		services.AddKafkaConsumer("EmailConsumer", sp =>
		{
			var kafkaOptions = sp.GetRequiredService<IOptions<KafkaSettings>>().Value;
			return new ConsumerConfig
			{
				BootstrapServers = kafkaOptions.Server,
				GroupId = kafkaOptions.GroupId,
				AutoOffsetReset = AutoOffsetReset.Earliest,
				EnableAutoCommit = true,
				EnablePartitionEof = true
			};
		});

		return services;
	}
}