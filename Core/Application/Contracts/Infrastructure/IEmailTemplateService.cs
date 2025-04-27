using Domain;

namespace Application.Contracts.Infrastructure;

public interface IEmailTemplateBuilder
{
	Task<Email> CreateConfirmEmail(string baseUrl,string userId);
	Task<Email> CreateDonationRequestEmail(string email, string city, double latitude, double longitude);
}
