using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

using Application.Contracts.Identity;
using Application.Exceptions;

using Domain;

using AutoMapper;


namespace Identity.Services;

public class AuthenticationService : IAuthenticationService
{
	private readonly SignInManager<IdentityUser> _signInManager;
	private readonly UserManager<IdentityUser> _userManager;
	private readonly ILogger<AuthenticationService> _logger;
	private readonly IMapper _mapper;

	public AuthenticationService(
		SignInManager<IdentityUser> signInManager,
		UserManager<IdentityUser> userManager,
		ILogger<AuthenticationService> logger,
		IMapper mapper)
	{
		_signInManager = signInManager;
		_userManager = userManager;
		_mapper = mapper;
		_logger = logger;
	}

	public async Task<User> AuthenticateAsync(string email, string password)
	{
		_logger.LogInformation("Attempting to authenticate user with email {Email}", email);

		var identityUser = await _userManager.FindByEmailAsync(email);
		if (identityUser is null)
		{
			_logger.LogWarning("Authentication failed: user with email {Email} not found", email);
			throw new InvalidCredentialsException(email);
		}

		var result = await _signInManager
			.CheckPasswordSignInAsync(identityUser, password, lockoutOnFailure: false);

		if (!result.Succeeded)
		{
			_logger.LogWarning("Authentication failed: invalid password for user {Email}", email);
			throw new InvalidCredentialsException(email);
		}

		_logger.LogInformation("User {Email} successfully authenticated", email);
		return _mapper.Map<User>(identityUser);
	}

	public async Task<User> RegisterAsync(string userName, string email, string password)
	{
		_logger.LogInformation("Attempting to register new user {UserName}, {Email}", userName, email);

		if (await _userManager.FindByNameAsync(userName) is not null)
		{
			_logger.LogWarning("Registration failed: username {UserName} already exists", userName);
			throw new UsernameAlreadyExistsException(userName);
		}

		if (await _userManager.FindByEmailAsync(email) is not null)
		{
			_logger.LogWarning("Registration failed: email {Email} already exists", email);
			throw new EmailAlreadyExistsException(email);
		}

		var identityUser = new IdentityUser
		{
			UserName = userName,
			Email = email,
			EmailConfirmed = false
		};

		var createResult = await _userManager.CreateAsync(identityUser, password);
		if (!createResult.Succeeded)
		{
			_logger.LogError("User creation failed for {Email}: {Errors}",
				email, string.Join(", ", createResult.Errors.Select(e => e.Description)));
			throw new UserCreationFailedException();
		}

		await _userManager.AddToRoleAsync(identityUser, "User");

		_logger.LogInformation("User {Email} successfully registered", email);
		return _mapper.Map<User>(identityUser);
	}

	public async Task ConfirmEmailAsync(string userId, string token)
	{
		_logger.LogInformation("Attempting to confirm email for user {UserId}", userId);

		var identityUser = await _userManager.FindByIdAsync(userId);
		if (identityUser is null)
		{
			_logger.LogWarning("Email confirmation failed: user {UserId} not found", userId);
			throw new UserNotFoundException();
		}

		if (identityUser.EmailConfirmed)
		{
			_logger.LogWarning("Email confirmation failed: user {UserId} already confirmed email", userId);
			throw new EmailAlreadyConfirmedException();
		}

		var result = await _userManager.ConfirmEmailAsync(identityUser, token);
		if (!result.Succeeded)
		{
			_logger.LogError("Email confirmation failed for user {UserId}", userId);
			throw new EmailConfirmationFailedException();
		}

		_logger.LogInformation("Email successfully confirmed for user {UserId}", userId);
	}

	public async Task<User> GetUserByIdAsync(string userId)
	{
		_logger.LogInformation("Retrieving user by ID {UserId}", userId);

		var appUser = await _userManager.FindByIdAsync(userId);
		if (appUser is null)
		{
			_logger.LogWarning("User {UserId} not found", userId);
			throw new UserNotFoundException();
		}

		return _mapper.Map<User>(appUser);
	}

	public async Task<string> GenerateConfirmEmailTokenAsync(string userId)
	{
		_logger.LogInformation("Generating confirmation email token for user {UserId}", userId);

		var user = await _userManager.FindByIdAsync(userId);
		if (user is null)
		{
			_logger.LogWarning("Token generation failed: user {UserId} not found", userId);
			throw new UserNotFoundException();
		}

		var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
		_logger.LogInformation("Generated email confirmation token for user {UserId}", userId);

		return token;
	}
}