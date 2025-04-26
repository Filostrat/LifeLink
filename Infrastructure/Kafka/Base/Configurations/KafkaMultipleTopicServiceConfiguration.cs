namespace Kafka.Base.Configurations
{
	public class KafkaMultipleTopicServiceConfiguration
	{
		public string SourceTopics { get; set; }
		public string RetryTopic { get; set; }
		public string ErrorTopic { get; set; }
		public int Delay { get; set; }
	}
}