using Api.Services.Contracts;

using Application.Constants;

using System.Security.Claims;


namespace Api.Services;

public class HttpContextService : IHttpContextService
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public HttpContextService(IHttpContextAccessor httpContextAccessor)
		=> _httpContextAccessor = httpContextAccessor;

	public string? UserId
		=> _httpContextAccessor.HttpContext?
			  .User.FindFirst(CustomClaimTypes.Uid)?.Value;

	public string? Email
		=> _httpContextAccessor.HttpContext?
			  .User.FindFirst(ClaimTypes.Email)?.Value;
}
