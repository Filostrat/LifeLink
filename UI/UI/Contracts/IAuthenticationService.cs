using UI.Models.Authentication;

namespace UI.Contracts;

public interface IAuthenticationService
{
	Task<AuthenticateRequestVM> Authenticate(string email, string password);
	Task<AuthenticateRequestVM> Register(RegisterVM registration);
	Task SendConfirmationEmailLink();
	Task Logout();
	bool IsEmailConfirmed();
	Task<AuthenticateRequestVM> ConfirmEmail(string token);
}