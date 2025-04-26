using Application.DTOs.Account.Responses;
using MediatR;


namespace Application.Features.Accounts.Requests.Commands;

public class ConfirmEmailCommand : IRequest<LoginAccountResponseDTO>
{
	public string UserId { get; set; }

	public string Token { get; set; }

	public ConfirmEmailCommand(string userId, string token)
	{
		UserId = userId;
		Token = token;
	}

}
