using System.Text.RegularExpressions;

namespace UI.Extensions;

public class ApiClientLoggingHandler : DelegatingHandler
{
	private readonly ILogger<ApiClientLoggingHandler> _logger;

	public ApiClientLoggingHandler(ILogger<ApiClientLoggingHandler> logger)
	{
		_logger = logger;
	}
	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		string requestBody = "";
		if (request.Content != null)
		{
			requestBody = await request.Content.ReadAsStringAsync(cancellationToken);
		}

		HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

		string responseBody = "";
		if (response.Content != null)
		{
			responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
		}

		_logger.LogInformation($"\nClient Request: {DecodeUnicodeSequences(requestBody)}\nClient Response: {DecodeUnicodeSequences(responseBody)}");

		return response;
	}

	private string DecodeUnicodeSequences(string input)
	{
		return Regex.Unescape(input);
	}
}