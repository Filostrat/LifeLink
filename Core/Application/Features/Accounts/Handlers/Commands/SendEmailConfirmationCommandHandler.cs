using Application.Contracts.Infrastructure;
using Application.Exceptions;
using Application.Features.Accounts.Requests.Commands;
using MediatR;

using Microsoft.Extensions.Logging;


namespace Application.Features.Accounts.Handlers.Commands;

public class SendEmailConfirmationCommandHandler : IRequestHandler<SendEmailConfirmationCommand, Unit>
{
	private readonly ILogger<SendEmailConfirmationCommandHandler> _logger;
	private readonly IEmailTemplateBuilder _emailTemplateBuilder;
	private readonly IMessageBus _messageBus;

	public SendEmailConfirmationCommandHandler(ILogger<SendEmailConfirmationCommandHandler> logger,
											   IEmailTemplateBuilder emailTemplateBuilder,
											   IMessageBus messageBus)
	{
		_emailTemplateBuilder = emailTemplateBuilder;
		_messageBus = messageBus;
		_logger = logger;
	}

	public async Task<Unit> Handle(SendEmailConfirmationCommand request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("Sending email confirmation for user {UserId}", request.UserId);

		var email = await _emailTemplateBuilder.CreateConfirmEmail(
			request.BaseUrl, request.UserId);

		if (!await _messageBus.PublishAsync(email, cancellationToken))
		{
			throw new EmailNotSendException();
		}

		return Unit.Value;
	}
}