using System;

using Kafka.Base.Interfaces;

namespace Kafka.Factories.Interfaces
{
	public interface IProducerFactory
	{
		IBaseProducer GetProducer(string producerName);
		IBaseProducer GetProducer<T>() where T : IBaseProducer;
	}
}