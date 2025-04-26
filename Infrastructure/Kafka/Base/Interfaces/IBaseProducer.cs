using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kafka.Base.Interfaces
{
	public interface IBaseProducer
	{
		string Name { get; }
		bool Send(string topic, string message);
		bool SendFlush(string topic, string message, TimeSpan timeSpan);
		Task<bool> SendAsync(string topic, string message, CancellationToken cancellationToken);
		Task<bool> SendToSinglePartitionAsync(string topic, int partition, string message, CancellationToken cancellationToken);
	}
}