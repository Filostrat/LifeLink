namespace Kafka.Base.Configurations
{
	public class KafkaServiceConfiguration
	{
		public string SourceTopic { get; set; }
		public string RetryTopic { get; set; }
		public string ErrorTopic { get; set; }
		public int Delay { get; set; }
	}
}