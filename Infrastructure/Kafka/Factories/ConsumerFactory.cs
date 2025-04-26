using System;
using System.Collections.Generic;
using System.Linq;
using Kafka.Base.Interfaces;
using Kafka.Factories.Interfaces;

using Microsoft.Extensions.Logging;

namespace Kafka.Factories
{
	public class ConsumerFactory : IConsumerFactory
	{
		private readonly IEnumerable<IBaseConsumer> _consumers;
		private readonly ILogger<ConsumerFactory> _logger;

		public ConsumerFactory(IEnumerable<IBaseConsumer> consumers, 
						       ILogger<ConsumerFactory> logger)
		{
			_consumers = consumers;
			_logger = logger;
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