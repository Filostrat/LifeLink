using Application.DTOs.Account.Responses;
using MediatR;


namespace Application.Features.Accounts.Requests.Commands;

public class RegisterAccountCommand : IRequest<LoginAccountResponseDTO>
{
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public int BloodTypeId { get; set; }
	public string Email { get; set; }
	public string UserName { get; set; }
	public string Password { get; set; }
}