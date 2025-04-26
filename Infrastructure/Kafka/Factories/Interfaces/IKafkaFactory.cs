using System;

using Kafka.Base.Interfaces;

namespace Kafka.Factories.Interfaces
{
	public interface IKafkaFactory
	{
		IBaseProducer GetProducer(string producerName);
		IBaseProducer GetProducer<T>() where T : IBaseProducer;
		IBaseConsumer GetConsumer(string consumerName);
		IBaseConsumer GetConsumer<T>() where T : IBaseConsumer;
	}
}