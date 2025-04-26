using Application.Constants;
using Application.Contracts.Identity;

using Domain;
using Domain.Settings;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Services;

public class JwtService : IJwtService
{
	private readonly JwtSettings _settings;
	private readonly UserManager<IdentityUser> _userManager;

	public JwtService(IOptions<JwtSettings> settings,
					  UserManager<IdentityUser> userManager)
	{
		_settings = settings.Value;
		_userManager = userManager;
	}

	public async Task<string> GenerateToken(User user)
	{
		var identityUser = await _userManager.FindByIdAsync(user.Id);

		var userClaims = await _userManager.GetClaimsAsync(identityUser);
		var roles = await _userManager.GetRolesAsync(identityUser);

		var claims = new List<Claim>
		{
			new(JwtRegisteredClaimNames.Sub, identityUser.UserName),
			new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			new(JwtRegisteredClaimNames.Email, identityUser.Email),
			new(CustomClaimTypes.EmailConfirmed, identityUser.EmailConfirmed.ToString()),
			new(CustomClaimTypes.Uid, identityUser.Id)
		};

		claims.AddRange(userClaims);
		claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var jwt = new JwtSecurityToken(
			issuer: _settings.Issuer,
			audience: _settings.Audience,
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(_settings.DurationInMinutes),
			signingCredentials: creds);

		return new JwtSecurityTokenHandler().WriteToken(jwt);
	}
}

