using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Kafka.Base.Models
{
	public class ErrorData
	{
		[JsonProperty("serice_name")]
		public string ServiceName { get; set; }

		[JsonProperty("error_message")]
		public string ErrorMessage { get; set; }

		[JsonProperty("stack_trace")]
		public string StackTrace { get; set; }

		[JsonProperty("data")]
		public string Data { get; set; }
	}
}