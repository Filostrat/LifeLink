namespace Application.Contracts.Infrastructure;

public interface IEmailTemplateBuilder
{
	Task<bool> CreateConfirmEmail(string baseUrl,string userId, CancellationToken cancellationToken);
	Task<bool> CreateDonationRequestEmailTemplate(string email, string city, double latitude, double longitude, CancellationToken cancellationToken);
}
