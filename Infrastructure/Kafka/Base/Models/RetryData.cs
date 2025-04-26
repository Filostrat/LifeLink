using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Kafka.Base.Models
{
	public class RetryData
	{
		[JsonProperty("destination")]
		public string DestinationTopic { get; set; }

		[JsonProperty("data")]
		public string Data { get; set; }
	}
}