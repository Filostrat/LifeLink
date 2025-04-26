using System;
using System.Collections.Generic;
using System.Linq;

using Kafka.Base.Interfaces;
using Kafka.Factories.Interfaces;

using Microsoft.Extensions.Logging;

namespace Kafka.Factories
{
	public class ProducerFactory : IProducerFactory
	{
		private readonly IEnumerable<IBaseProducer> _producers;
		private readonly ILogger<ProducerFactory> _logger;

		public ProducerFactory(IEnumerable<IBaseProducer> producers, 
						       ILogger<ProducerFactory> logger)
		{
			_producers = producers;
			_logger = logger;
		}

		public IBaseProducer GetProducer(string producerName)
		{
			return _producers.FirstOrDefault(c => c.Name == producerName);
		}

		public IBaseProducer GetProducer<T>() where T : IBaseProducer
		{
			return _producers.OfType<T>().FirstOrDefault();
		}
	}
}