using System;

using Kafka.Base.Interfaces;

namespace Kafka.Factories.Interfaces
{
	public interface IConsumerFactory
	{
		IBaseConsumer GetConsumer(string consumerName);
		IBaseConsumer GetConsumer<T>() where T : IBaseConsumer;
	}
}