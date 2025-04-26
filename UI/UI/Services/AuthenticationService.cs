using Microsoft.AspNetCore.Authentication.Cookies;
using IAuthenticationService = UI.Contracts.IAuthenticationService;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using UI.Contracts;
using UI.Models.Authentication;
using UI.Services.Base;

using AutoMapper; 
using Microsoft.AspNetCore.Authentication;


namespace UI.Services;

public class AuthenticationService : BaseHttpService, IAuthenticationService
{
	private readonly IClient _client;
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly JwtSecurityTokenHandler _tokenHandler;
	private readonly IMapper _mapper;

	public AuthenticationService(
		IClient client,
		ILocalStorageService localStorageService,
		IHttpContextAccessor httpContextAccessor,
		JwtSecurityTokenHandler tokenHandler,
		IMapper mapper) : base(client, localStorageService)
	{
		_client = client;
		_httpContextAccessor = httpContextAccessor;
		_tokenHandler = tokenHandler;
		_mapper = mapper;
	}

	public async Task<AuthenticateRequestVM> Authenticate(string email, string password)
	{
		var authRequest = new LoginAccountRequestDTO { Email = email, Password = password };

		try
		{
			var response = await _client.LoginAsync(authRequest);
			return await HandleAuthenticationResponse(response.Result);
		}
		catch
		{
			return FailedAuthResult();
		}
	}

	public async Task<AuthenticateRequestVM> Register(RegisterVM registration)
	{
		try
		{
			var request = _mapper.Map<RegisterAccountRequestDTO>(registration);
			var response = await _client.RegisterAsync(request);
			return await HandleAuthenticationResponse(response.Result);
		}
		catch
		{
			return FailedAuthResult();
		}
	}

	public async Task SendConfirmationEmailLink()
	{
		var baseUrl = BuildBaseUrl();

		var confirmationUrl = $"{baseUrl}/Users/EmailConfirmed/confirm-email";

		AddBearerToken();
		await _client.SendConfirmationAsync(confirmationUrl);
	}


	public async Task<AuthenticateRequestVM> ConfirmEmail(string token)
	{
		try
		{
			AddBearerToken();
			var response = await _client.ConfirmEmailAsync(token);
			return await HandleAuthenticationResponse(response.Result);
		}
		catch
		{
			return FailedAuthResult();
		}
	}

	public async Task Logout()
	{
		_localStorage.ClearStorage(["token"]);
		await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
	}

	private async Task<AuthenticateRequestVM> HandleAuthenticationResponse(LoginAccountResponseDTO loginAccountResponseDTO)
	{
		if (string.IsNullOrEmpty(loginAccountResponseDTO?.Token))
		{
			return new AuthenticateRequestVM
			{
				Succeeded = false,
				EmailConfirmed = loginAccountResponseDTO?.EmailConfirmed ?? false
			};
		}

		var tokenContent = _tokenHandler.ReadJwtToken(loginAccountResponseDTO.Token);
		var claims = ParseClaims(tokenContent);
		var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));

		await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);
		_localStorage.SetStorageValue("token", loginAccountResponseDTO.Token);

		return new AuthenticateRequestVM
		{
			Succeeded = true,
			EmailConfirmed = loginAccountResponseDTO.EmailConfirmed
		};
	}

	private IList<Claim> ParseClaims(JwtSecurityToken token)
	{
		var claims = token.Claims.ToList();
		if (!string.IsNullOrWhiteSpace(token.Subject))
		{
			claims.Add(new Claim(ClaimTypes.Name, token.Subject));
		}
		return claims;
	}

	private AuthenticateRequestVM FailedAuthResult() => new()
	{
		Succeeded = false,
		EmailConfirmed = false
	};

	private string BuildBaseUrl()
	{
		var request = _httpContextAccessor.HttpContext.Request;
		return $"{request.Scheme}://{request.Host}{request.PathBase}";
	}

	public bool IsEmailConfirmed()
	{
		if (!_localStorage.Exists("token"))
			return false;

		var token = _localStorage.GetStorageValue<string>("token");

		var jwt = _tokenHandler.ReadJwtToken(token);

		var emailConfirmedClaim = jwt.Claims.FirstOrDefault(c => c.Type == "emailConfirmed");

		if (emailConfirmedClaim == null)
			return false;

		return bool.TryParse(emailConfirmedClaim.Value, out var isConfirmed) && isConfirmed;
	}
}