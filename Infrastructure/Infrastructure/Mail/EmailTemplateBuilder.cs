using Application.Contracts.Identity;
using Application.Contracts.Infrastructure;

using Domain;
using System.Web;


namespace Infrastructure.Mail;

public class EmailTemplateBuilder : IEmailTemplateBuilder
{
	private readonly IAuthenticationService _authenticationService;

	public EmailTemplateBuilder(IAuthenticationService authenticationService)
	{
		_authenticationService = authenticationService;
	}

	public async Task<Email> CreateConfirmEmail(string baseUrl, string userId)
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

		return emailMessage;
	}

	public async Task<Email> CreateDonationRequestEmail(string email, string city, double latitude, double longitude)
	{
		string googleLocationLink = $"https://www.google.com/maps/search/?api=1&query={latitude},{longitude}";

		var emailMessage = new Email
		{
			To = email,
			Subject = "Потрібна твоя допомога",
			Body = $"В місті {city} за <a href=\"{googleLocationLink}\">локацією</a> потрібна саме твоя кров!"
		};

		return emailMessage;
	}
}