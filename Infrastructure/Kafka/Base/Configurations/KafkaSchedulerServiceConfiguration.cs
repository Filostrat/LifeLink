namespace Kafka.Base.Configurations
{
	public class KafkaScheduledServiceConfiguration
	{
		public string SourceTopic { get; set; }
		public string ErrorTopic { get; set; }
		public string RetryTopic { get; set; }
		public string StartTime { get; set; }
		public string EndTime { get; set; }
		public int Delay { get; set; } = 0;
	}
}