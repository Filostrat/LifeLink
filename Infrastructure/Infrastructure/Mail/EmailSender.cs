using Application.Contracts.Infrastructure;
using Domain;
using Domain.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System.Net;
using System.Net.Mail;


namespace Infrastructure.Mail;

public class EmailSender : IEmailSender
{
	private readonly IOptions<EmailSettings> _emailSettings;
	private readonly ILogger<EmailSender> _logger;

	public EmailSender(IOptions<EmailSettings> emailSettings, ILogger<EmailSender> logger)
	{
		_emailSettings = emailSettings;
		_logger = logger;
	}

	public async Task<bool> SendEmailAsync(Email email)
	{
		_logger.LogInformation("Preparing to send email to {ToEmail} with subject: {Subject}", email.To, email.Subject);

		using var client = new SmtpClient(_emailSettings.Value.Host, _emailSettings.Value.Port)
		{
			Credentials = new NetworkCredential(_emailSettings.Value.UserName, _emailSettings.Value.Password),
			EnableSsl = _emailSettings.Value.EnableSsl
		};

		var mail = new MailMessage
		{
			From = new MailAddress(_emailSettings.Value.SenderEmail, _emailSettings.Value.SenderName),
			Subject = email.Subject,
			Body = email.Body,
			IsBodyHtml = true
		};

		mail.To.Add(email.To);

		await client.SendMailAsync(mail);

		_logger.LogInformation("Email successfully sent to {ToEmail}", email.To);
		return true;
	}
}