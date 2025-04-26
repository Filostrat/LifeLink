namespace Application.DTOs.Account.Requests;

public class RegisterAccountRequestDTO
{
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public int BloodTypeId { get; set; }
	public string Email { get; set; }
	public string UserName { get; set; }
	public string Password { get; set; }
}