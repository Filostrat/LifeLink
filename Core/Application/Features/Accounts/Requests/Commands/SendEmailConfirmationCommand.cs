using MediatR;


namespace Application.Features.Accounts.Requests.Commands;

public class SendEmailConfirmationCommand : IRequest<Unit>
{
	public string BaseUrl { get; set; }
	public string UserId { get; set; }

	public SendEmailConfirmationCommand(string baseUrl, string userId)
	{
		BaseUrl = baseUrl;
		UserId = userId;
	}
}

