namespace Domain.Settings;

public class KafkaSettings
{
	public string Server { get; set; }
	public string GroupId { get; set; }
	public string TopicEmail { get; set; }
	public string TopicTelegram { get; set; }
}
