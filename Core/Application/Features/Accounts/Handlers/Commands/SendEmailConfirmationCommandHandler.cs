using Application.Contracts.Infrastructure;
using Application.Exceptions;
using Application.Features.Accounts.Requests.Commands;
using MediatR;

using Microsoft.Extensions.Logging;


namespace Application.Features.Accounts.Handlers.Commands;

public class SendEmailConfirmationCommandHandler : IRequestHandler<SendEmailConfirmationCommand, Unit>
{
	private readonly IEmailTemplateBuilder _emailTemplateBuilder;
	private readonly ILogger<SendEmailConfirmationCommandHandler> _logger;

	public SendEmailConfirmationCommandHandler(IEmailTemplateBuilder emailTemplateBuilder,
											   ILogger<SendEmailConfirmationCommandHandler> logger)
	{
		_emailTemplateBuilder = emailTemplateBuilder;
		_logger = logger;
	}

	public async Task<Unit> Handle(SendEmailConfirmationCommand request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("Sending email confirmation for user {UserId}", request.UserId);

		if (!await _emailTemplateBuilder.CreateConfirmEmail(request.BaseUrl, request.UserId, cancellationToken))
		{
			throw new EmailNotSendException();
		}

		return Unit.Value;
	}
}
