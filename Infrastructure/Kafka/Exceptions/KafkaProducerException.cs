using System;

namespace Kafka.Exceptions
{
	[Serializable]
	public class KafkaProducerException : Exception
	{
		public ProducerExceptionData ProducerExceptionData { get; }

		public KafkaProducerException()
		{

		}

		public KafkaProducerException(string message, ProducerExceptionData data) : base(message)
		{
			ProducerExceptionData = data;
		}

		public KafkaProducerException(string message, ProducerExceptionData data, Exception innerException) : base(message, innerException)
		{
			ProducerExceptionData = data;
		}
	}

	public class ProducerExceptionData
	{
		public string Server { get; init; }
		public string GroupId { get; init; }
		public string Topic { get; init; }
		public int? Partition { get; set; }
		public string ProduceMessage { get; init; }
	}
}