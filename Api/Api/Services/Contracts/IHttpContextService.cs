namespace Api.Services.Contracts;

public interface IHttpContextService
{
	string? UserId { get; }
	string? Email { get; }
}

