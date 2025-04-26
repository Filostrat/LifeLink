using System;
using System.Threading;
using System.Threading.Tasks;

using Confluent.Kafka;

using Kafka.Base.Interfaces;
using Kafka.Exceptions;

using Microsoft.Extensions.Logging;

namespace Kafka.Base
{
	public class BaseProducer : IBaseProducer, IDisposable
	{
		public string Name => _name;

		private readonly string _name;
		private readonly ProducerConfig _producerConfig;
		private readonly IProducer<Null, string> _producer;
		private readonly ILogger<BaseProducer> _logger;


		public BaseProducer(string name, ProducerConfig producerConfig, ILogger<BaseProducer> logger)
		{
			_name = name;
			_producerConfig = producerConfig;
			_logger = logger;

			ProducerBuilder<Null, string> pb = new(producerConfig);
			_producer = pb.Build();
		}

		public void Dispose()
		{
			_producer.Dispose();
		}

		public async Task<bool> SendAsync(string topic, string message, CancellationToken cancellationToken)
		{
			try
			{
				await _producer.ProduceAsync(topic, new Message<Null, string> { Value = message }, cancellationToken);

				_logger.LogInformation("Message was produced to {topic} topic", topic);

				return true;
			}
			catch (Exception e)
			{
				_logger.LogError("{errorMessage}", e.Message);
				_logger.LogError("Server: {server}\nGroupId: {groupId}\nTopic: {topic}\nMessage: {message}",
					_producerConfig.BootstrapServers, _producerConfig.ClientId, topic, message);
				_logger.LogError("{errorStackTrace}", e.StackTrace);

				return false;
			}
		}

		public async Task<bool> SendToSinglePartitionAsync(string topic, int partition, string message, CancellationToken cancellationToken)
		{
			try
			{
				await _producer.ProduceAsync(new TopicPartition(topic, new Partition(partition)), new Message<Null, string> { Value = message }, cancellationToken);

				_logger.LogInformation("Message was produced to {topic} topic", topic);

				return true;
			}
			catch (Exception e)
			{
				_logger.LogError("{errorMessage}", e.Message);
				_logger.LogError("Server: {server}\nGroupId: {groupId}\nTopic: {topic}\nPartiotion: {partition}\nMessage: {message}",
					_producerConfig.BootstrapServers, _producerConfig.ClientId, topic, partition, message);
				_logger.LogError("{errorStackTrace}", e.StackTrace);

				return false;
			}
		}

		public bool Send(string topic, string message)
		{
			try
			{
				_producer.Produce(topic, new Message<Null, string> { Value = message });

				_logger.LogInformation("Message was produced to {topic} topic", topic);

				return true;
			}
			catch (Exception e)
			{
				_logger.LogError("{errorMessage}", e.Message);
				_logger.LogError("Server: {server}\nGroupId: {groupId}\nTopic: {topic}\nMessage: {message}",
					_producerConfig.BootstrapServers, _producerConfig.ClientId, topic, message);
				_logger.LogError("{errorStackTrace}", e.StackTrace);

				return false;
			}
		}

		public bool SendFlush(string topic, string message, TimeSpan timeSpan)
		{
			try
			{
				_producer.Produce(topic, new Message<Null, string> { Value = message });
				_producer.Flush(timeSpan);

				_logger.LogInformation("Message was produced to {topic} topic", topic);

				return true;
			}
			catch (Exception e)
			{
				_logger.LogError("{errorMessage}", e.Message);
				_logger.LogError("Server: {server}\nGroupId: {groupId}\nTopic: {topic}\nMessage: {message}",
					_producerConfig.BootstrapServers, _producerConfig.ClientId, topic, message);
				_logger.LogError("{errorStackTrace}", e.StackTrace);

				return false;
			}
		}
	}
}