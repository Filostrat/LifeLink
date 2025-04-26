using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Confluent.Kafka;

using Kafka.Base.Interfaces;

using Microsoft.Extensions.Logging;

namespace Kafka.Base
{
	public class BaseConsumer : IBaseConsumer, IDisposable
	{
		public string Name => _name;

		private readonly string _name;
		private readonly IConsumer<Ignore, string> _consumer;

		private readonly ILogger<BaseConsumer> _logger;

		public BaseConsumer(string name, ConsumerConfig consumerConfig, ILogger<BaseConsumer> logger)
		{
			_name = name;
			_logger = logger;

			ConsumerBuilder<Ignore, string> cb = new ConsumerBuilder<Ignore, string>(consumerConfig)
				.SetErrorHandler((_, e) => _logger.LogError("Error: {error}", e.Reason))
				.SetPartitionsAssignedHandler((c, partitions) => _logger.LogInformation("Partitions assigned: {partitions}", string.Join(", ", partitions)))
				.SetPartitionsRevokedHandler((c, partitions) =>
				{
					_logger.LogInformation("Partitions revoked: {partitions}", string.Join(", ", partitions));
					IEnumerable<TopicPartition> remaining = c.Assignment.Where(atp => !partitions.Where(rtp => rtp.TopicPartition == atp).Any());
				})
				.SetPartitionsLostHandler((c, partitions) => _logger.LogInformation("Partitions lost: {partitions}", string.Join(", ", partitions)));

			_consumer = cb.Build();
		}

		public void Dispose()
		{
			_consumer.Close();
			_consumer.Dispose();
		}

		public void Subscribe(string topic)
		{
			_consumer.Subscribe(topic);
		}

		public void Subscribe(string[] topics)
		{
			_consumer.Subscribe(topics);
		}

		public async Task<ConsumeResult<Ignore, string>> ConsumeAsync(CancellationToken cancellationToken)
		{
			ConsumeResult<Ignore, string> consumeResult = null;

			while (!cancellationToken.IsCancellationRequested)
			{
				try
				{
					consumeResult = _consumer.Consume(cancellationToken);

					if (consumeResult is not null && !consumeResult.IsPartitionEOF)
					{
						if (consumeResult.Offset >= 0)
						{
							try
							{
								_consumer.StoreOffset(consumeResult);
								_consumer.Commit();
							}
							catch (KafkaException e)
							{
								_logger.LogError("Failed to commit offsets.\nTopic: {topic}\nPartition: {partition}\nOffset: {offset}\nError: {errorMessage}",
									consumeResult.Topic, consumeResult.Partition, consumeResult.Offset, e.Message);
								_logger.LogError("{errorStackTrace}", e.StackTrace);
							}
						}
						else
						{
							_logger.LogError("Invalid offset: {offset}", consumeResult.Offset);
						}

						return consumeResult;
					}
				}
				catch (KafkaException e)
				{
					_logger.LogError("{errorMessage}", e.Message);
					_logger.LogError("{errorStackTrace}", e.StackTrace);

					await Task.Delay(3000, cancellationToken);
				}
				catch (OperationCanceledException e)
				{
					_logger.LogError("Consuming was cancelled. {message}", e.Message);
					break;
				}
			}

			return consumeResult;
		}

		public async Task<ConsumeResult<Ignore, string>> ConsumeWithDelayAsync(TimeSpan delay, CancellationToken cancellationToken)
		{
			ConsumeResult<Ignore, string> consumeResult = null;

			while (!cancellationToken.IsCancellationRequested)
			{
				try
				{
					consumeResult = _consumer.Consume(cancellationToken);

					if (consumeResult is not null && !consumeResult.IsPartitionEOF)
					{
						TimeSpan delayTime = DateTime.UtcNow - consumeResult.Message.Timestamp.UtcDateTime;
						if (delayTime < delay)
						{
							await Task.Delay(delay - delayTime, cancellationToken);
						}

						if (consumeResult.Offset >= 0)
						{
							try
							{
								_consumer.StoreOffset(consumeResult);
								_consumer.Commit();
							}
							catch (KafkaException e)
							{
								_logger.LogError("Failed to commit offsets.\nTopic: {topic}\nPartition: {partition}\nOffset: {offset}\nError: {errorMessage}",
									consumeResult.Topic, consumeResult.Partition, consumeResult.Offset, e.Message);
								_logger.LogError("{errorStackTrace}", e.StackTrace);
							}
						}
						else
						{
							_logger.LogError("Invalid offset: {offset}", consumeResult.Offset);
						}

						return consumeResult;
					}
				}
				catch (KafkaException e)
				{
					_logger.LogError("{errorMessage}", e.Message);
					_logger.LogError("{errorStackTrace}", e.StackTrace);

					await Task.Delay(3000, cancellationToken);
				}
				catch (OperationCanceledException e)
				{
					_logger.LogError("Consuming was cancelled. {message}", e.Message);
				}
			}

			return consumeResult;
		}
	}
}