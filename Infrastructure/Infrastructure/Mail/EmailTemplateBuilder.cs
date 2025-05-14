using Application.Contracts.Identity;
using Application.Contracts.Infrastructure;
using Domain;
using System.Web;

namespace Infrastructure.Mail;

public class EmailTemplateBuilder : IEmailTemplateBuilder
{
	private readonly IAuthenticationService _authenticationService;

	public EmailTemplateBuilder(IAuthenticationService authenticationService)
	{
		_authenticationService = authenticationService;
	}

	public async Task<Email> CreateConfirmEmail(string baseUrl, string userId)
	{
		var token = await _authenticationService.GenerateConfirmEmailTokenAsync(userId);
		var tokenEncoded = HttpUtility.UrlEncode(token);
		var confirmationLink = $"{baseUrl}?token={tokenEncoded}";
		var user = await _authenticationService.GetUserByIdAsync(userId);

		var htmlBody = $@"
<!DOCTYPE html>
<html lang=""uk"">
  <head>
    <meta charset=""UTF-8"">
    <title>Підтвердіть вашу пошту</title>
  </head>
  <body style=""margin:0;padding:0;font-family:Arial,sans-serif;background:#e9ecef;"">
    <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background:#e9ecef;padding:50px 0;"">
      <tr>
        <td align=""center"">
          <table width=""600"" cellpadding=""0"" cellspacing=""0"" style=""background:#ffffff;border-radius:8px;overflow:hidden;box-shadow:0 4px 12px rgba(0,0,0,0.1);"">
            <tr>
              <td style=""padding:20px;text-align:center;background:#dc3545;color:white;"">
                <h1 style=""margin:0;font-size:24px;"">Підтвердіть вашу електронну пошту</h1>
              </td>
            </tr>
            <tr>
              <td style=""padding:30px;color:#333333;"">
                <p style=""font-size:16px;line-height:1.5;"">
                  Привіт, {HttpUtility.HtmlEncode(user.UserName)}!
                </p>
                <p style=""font-size:16px;line-height:1.5;"">
                  Щоб завершити реєстрацію, натисніть на кнопку нижче:
                </p>
                <p style=""text-align:center;margin:30px 0;"">
                  <a href=""{confirmationLink}"" 
                     style=""display:inline-block;padding:12px 24px;background:#dc3545;color:#ffffff;text-decoration:none;border-radius:4px;font-weight:bold;"">
                    Підтвердити пошту
                  </a>
                </p>
                <p style=""font-size:14px;color:#777777;line-height:1.4;"">
                  Якщо ви не реєструвалися на нашому сайті, просто проігноруйте цей лист.
                </p>
              </td>
            </tr>
            <tr>
              <td style=""padding:20px;text-align:center;font-size:12px;color:#aaaaaa;"">
                © 2025 LifeLink. Всі права захищені.
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
  </body>
</html>";

		return new Email
		{
			To = user.Email,
			Subject = "Підтвердження електронної пошти",
			Body = htmlBody
		};
	}

	public async Task<Email> CreateDonationRequestEmail(string email, string city, double latitude, double longitude,string message)
	{
		string googleLocationLink = $"https://www.google.com/maps/search/?api=1&query={latitude},{longitude}";

		var htmlBody = $@"
<!DOCTYPE html>
<html lang=""uk"">
  <head>
    <meta charset=""UTF-8"">
    <title>Потрібна твоя допомога</title>
  </head>
  <body style=""margin:0;padding:0;font-family:Arial,sans-serif;background:#e9ecef;"">
    <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background:#e9ecef;padding:50px 0;"">
      <tr>
        <td align=""center"">
          <table width=""600"" cellpadding=""0"" cellspacing=""0"" style=""background:#ffffff;border-radius:8px;overflow:hidden;box-shadow:0 4px 12px rgba(0,0,0,0.1);"">
            <tr>
              <td style=""padding:20px;text-align:center;background:#dc3545;color:white;"">
                <h1 style=""margin:0;font-size:24px;"">Потрібна твоя допомога</h1>
              </td>
            </tr>
            <tr>
              <td style=""padding:30px;color:#333333;"">
                <p style=""font-size:16px;line-height:1.5;"">
                  В місті <strong>{HttpUtility.HtmlEncode(city)}</strong> потрібна саме твоя кров!
                </p>
                <p style=""font-size:16px;line-height:1.5;"">
                  {message}
                </p>
                <p style=""text-align:center;margin:30px 0;"">
                  <a href=""{googleLocationLink}"" 
                     style=""display:inline-block;padding:12px 24px;background:#dc3545;color:#ffffff;text-decoration:none;border-radius:4px;font-weight:bold;"">
                    Переглянути локацію
                  </a>
                </p>
                <p style=""font-size:14px;color:#777777;line-height:1.4;"">
                  Якщо ви більше не бажаєте отримувати такі повідомлення, просто проігноруйте цей лист.
                </p>
              </td>
            </tr>
            <tr>
              <td style=""padding:20px;text-align:center;font-size:12px;color:#aaaaaa;"">
                © 2025 LifeLink. Всі права захищені.
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
  </body>
</html>";

		return new Email
		{
			To = email,
			Subject = "Потрібна твоя допомога – донорська кров",
			Body = htmlBody
		};
	}
}