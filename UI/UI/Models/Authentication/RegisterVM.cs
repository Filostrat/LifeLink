namespace UI.Models.Authentication;

public class RegisterVM
{
	public string FirstName { get; set; }

	public string LastName { get; set; }

	public string Email { get; set; }

	public string UserName { get; set; }

	public string BloodTypeId { get; set; }

	public string Password { get; set; }

	public string ConfirmPassword { get; set; }
}