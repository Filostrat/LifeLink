using System;
using System.Threading;
using System.Threading.Tasks;

using Confluent.Kafka;

namespace Kafka.Base.Interfaces
{
	public interface IBaseConsumer
	{
		string Name { get; }
		Task<ConsumeResult<Ignore, string>> ConsumeAsync(CancellationToken cancellationToken);
		Task<ConsumeResult<Ignore, string>> ConsumeWithDelayAsync(TimeSpan delay, CancellationToken cancellationToken);
		void Subscribe(string topic);
		void Subscribe(string[] topics);
	}
}