using Domain;

namespace Application.Contracts.Infrastructure;

public interface IEmailSender
{
	Task<bool> SendEmailAsync(Email email);
}

