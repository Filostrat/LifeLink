using Domain;


namespace Application.Contracts.Identity;

public interface IJwtService
{
	Task<string> GenerateToken(User user);
}
