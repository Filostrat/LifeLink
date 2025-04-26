namespace Application.DTOs.Account.Responses;

public class LoginAccountResponseDTO
{
	public string Id { get; set; }
	public string UserName { get; set; }
	public string Email { get; set; }
	public string Token { get; set; }
	public bool EmailConfirmed { get; set; }
}