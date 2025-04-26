using Application.DTOs.Account.Responses;
using MediatR;


namespace Application.Features.Accounts.Requests.Commands;

public class LoginAccountCommand : IRequest<LoginAccountResponseDTO>
{
	public string Email { get; set; }
	public string Password { get; set; }
}