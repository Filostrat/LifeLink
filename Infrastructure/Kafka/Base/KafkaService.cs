using System;
using System.Threading;
using System.Threading.Tasks;

using Confluent.Kafka;

using Kafka.Base.Configurations;
using Kafka.Base.Interfaces;
using Kafka.Base.Models;
using Kafka.Factories.Interfaces;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

namespace Kafka.Base
{
	public abstract class KafkaService : BackgroundService
	{
		protected ILogger<KafkaService> _logger;
		protected readonly IBaseConsumer _serviceConsumer;
		protected readonly IBaseProducer _serviceProducer;
		private readonly KafkaServiceConfiguration _configuration;

		public KafkaService(IOptions<KafkaServiceConfiguration> configuration,
			IKafkaFactory kafkaFactory,
			ILogger<KafkaService> logger)
		{
			_configuration = configuration.Value;

			_serviceConsumer = kafkaFactory.GetConsumer("ServiceConsumer");
			_serviceProducer = kafkaFactory.GetProducer("ServiceProducer");

			_logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			await Task.Yield();

			_serviceConsumer.Subscribe(_configuration.SourceTopic);
			_logger.LogInformation("Subscribed on {topc}", _configuration.SourceTopic);

			ConsumeResult<Ignore, string> consumeResult = null;

			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					_logger.LogInformation("================================================================================");

					consumeResult = await _serviceConsumer.ConsumeWithDelayAsync(TimeSpan.FromSeconds(_configuration.Delay), stoppingToken);
					_logger.LogInformation("Consumed message {message}", consumeResult.Message.Value);

					await ExecuteInternalAsync(consumeResult.Message.Value, stoppingToken);
				}
				catch (Exception e)
				{
					string data = consumeResult is null && consumeResult.Message is null ? "" : consumeResult.Message.Value;

					_logger.LogError("{errorMessage}", e.Message);
					_logger.LogError("Record: {data}", data);
					_logger.LogError("{stackTrace}", e.StackTrace);

					ErrorData errorData = new()
					{
						ServiceName = GetType().Assembly.GetName().Name,
						ErrorMessage = e.Message,
						StackTrace = e.StackTrace,
						Data = data
					};

					_serviceProducer.SendFlush(_configuration.ErrorTopic, JsonConvert.SerializeObject(errorData), TimeSpan.FromSeconds(10));
				}
			}
		}

		protected abstract Task ExecuteInternalAsync(string consumedMessage, CancellationToken stoppingToken);
	}
}