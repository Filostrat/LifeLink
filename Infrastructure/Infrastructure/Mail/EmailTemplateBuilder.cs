using Application.Contracts.Identity;
using Application.Contracts.Infrastructure;

using Domain;
using System.Web;


namespace Infrastructure.Mail;

public class EmailTemplateBuilder : IEmailTemplateBuilder
{
	private readonly IAuthenticationService _authenticationService;
	private readonly IMessageBus _messageBus;

	public EmailTemplateBuilder(IAuthenticationService authenticationService,IMessageBus messageBus)
	{
		_authenticationService = authenticationService;
		_messageBus = messageBus;
	}

	public async Task<bool> CreateConfirmEmail(string baseUrl, string userId, CancellationToken cancellationToken)
	{
		var token = await _authenticationService.GenerateConfirmEmailTokenAsync(userId);

		var tokenEncoded = HttpUtility.UrlEncode(token);
		var confirmationLink = $"{baseUrl}?token={tokenEncoded}";

		var user = await _authenticationService.GetUserByIdAsync(userId);

		var emailMessage = new Email
		{
			To = user.Email,
			Subject = "Confirm your email",
			Body = $"Please confirm your email by clicking <a href=\"{confirmationLink}\">here</a>"
		};

		return await _messageBus.PublishAsync(emailMessage, cancellationToken);
	}

	public async Task<bool> CreateDonationRequestEmailTemplate(string email, string city, double latitude, double longitude, CancellationToken cancellationToken)
	{
		string googleLocationLink = $"https://www.google.com/maps/search/?api=1&query={latitude},{longitude}";

		var emailMessage = new Email
		{
			To = email,
			Subject = "Потрібна твоя допомога",
			Body = $"В місті {city} за <a href=\"{googleLocationLink}\">локацією</a> потрібна саме твоя кров!"
		};

		return await _messageBus.PublishAsync(emailMessage, cancellationToken);
	}
}