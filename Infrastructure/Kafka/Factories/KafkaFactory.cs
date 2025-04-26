using System;
using System.Collections.Generic;
using System.Linq;
using Kafka.Base.Interfaces;
using Kafka.Factories.Interfaces;

using Microsoft.Extensions.Logging;

namespace Kafka.Factories
{
	public class KafkaFactory : IKafkaFactory
	{
		private readonly ILogger<ConsumerFactory> _logger;
		private readonly IEnumerable<IBaseConsumer> _consumers;
		private readonly IEnumerable<IBaseProducer> _producers;

		public KafkaFactory(ILogger<ConsumerFactory> logger,
							IEnumerable<IBaseConsumer> consumers,
							IEnumerable<IBaseProducer> producers)
		{
			_logger = logger;
			_consumers = consumers;
			_producers = producers;
		}

		public IBaseProducer GetProducer(string producerName)
		{
			return _producers.FirstOrDefault(c => c.Name == producerName);
		}

		public IBaseProducer GetProducer<T>() where T : IBaseProducer
		{
			return _consumers.OfType<T>().FirstOrDefault();
		}

		public IBaseConsumer GetConsumer(string consumerName)
		{
			return _consumers.FirstOrDefault(c => c.Name == consumerName);
		}

		public IBaseConsumer GetConsumer<T>() where T : IBaseConsumer
		{
			return _consumers.OfType<T>().FirstOrDefault();
		}
	}
}