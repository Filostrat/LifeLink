using Domain;


namespace Application.Contracts.Identity;

public interface IAuthenticationService
{
	Task<User> AuthenticateAsync(string email, string password);
	Task<User> RegisterAsync(string userName, string email, string password);
	Task ConfirmEmailAsync(string userId, string token);
	Task<User> GetUserByIdAsync(string userId);
	Task<string> GenerateConfirmEmailTokenAsync(string userId);
}