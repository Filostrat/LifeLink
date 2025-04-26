using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UI.Contracts;
using UI.Models.Authentication;
using UI.Models.Donors;
using UI.Services;

namespace UI.Controllers;

public class UsersController : Controller
{
	private readonly IAuthenticationService _authService;
	private readonly IBloodTypeService _bloodTypeService;
	private readonly IDonorService _donorService;
	private readonly ILogger<UsersController> _logger;

	public UsersController(IAuthenticationService authService,IDonorService donorService, IBloodTypeService bloodTypeService, ILogger<UsersController> logger)
	{
		_donorService = donorService;
		_authService = authService;
		_bloodTypeService = bloodTypeService;
		_logger = logger;
	}

	public IActionResult Login(string returnUrl = null)
	{
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> Login(LoginVM login, string returnUrl = null)
	{
		_logger.LogInformation("Login attempt for email: {Email}", login.Email);

		if (!ModelState.IsValid)
		{
			_logger.LogWarning("ModelState is invalid for login attempt by email: {Email}", login.Email);
			return View(login);
		}

		var result = await _authService.Authenticate(login.Email, login.Password);

		if (result.Succeeded && !result.EmailConfirmed.Value)
		{
			_logger.LogInformation("Login succeeded but email not confirmed for user: {Email}", login.Email);
			return RedirectToAction("EmailNotConfirmed", new { email = login.Email });
		}
		else if (result.Succeeded)
		{
			_logger.LogInformation("Login succeeded for user: {Email}", login.Email);
			return RedirectToAction("Index", "Home");
		}

		_logger.LogWarning("Login failed for user: {Email}", login.Email);
		ModelState.AddModelError("", "Спроба входу не вдалася. Спробуйте ще раз.");
		return View(login);
	}


	[Authorize]
	public IActionResult EmailNotConfirmed()
	{
		return View();
	}


	[HttpPost]
	[Authorize]
	public async Task<IActionResult> EmailSent()
	{
		await _authService.SendConfirmationEmailLink();
		return View();
	}

	[HttpGet]
	[Authorize]
	public async Task<IActionResult> EmailConfirmed(string token)
	{
		var result = await _authService.ConfirmEmail(token);

		ViewBag.EmailConfirmed = result.Succeeded;

		return View("EmailConfirmationResult");
	}

	[HttpGet]
	[Authorize]
	public async Task<IActionResult> DonorInformation()
	{
		if (!_authService.IsEmailConfirmed())
		{
			return RedirectToAction(nameof(EmailNotConfirmed));
		}

		var myDonorInfo = await _donorService.DonorInformation();
		var types = await _bloodTypeService.GetBloodTypes();

		ViewBag.BloodTypes = new SelectList(types, "Id", "Type");

		return View(myDonorInfo);
	}

	[HttpPost]
	[Authorize]
	public async Task<IActionResult> UpdateDonorInformation(DonorVM donorVM)
	{
		var myDonorInfo = await _donorService.UpdateDonorInformation(donorVM);

		var types = await _bloodTypeService.GetBloodTypes();

		ViewBag.BloodTypes = new SelectList(types, "Id", "Type");

		return View(nameof(DonorInformation), myDonorInfo);
	}

	[HttpGet]
	public async Task<IActionResult> Register()
	{
		var types = await _bloodTypeService.GetBloodTypes();

		ViewBag.BloodTypes = new SelectList(types, "Id", "Type");

		return View(new RegisterVM());
	}

	[HttpPost]
	public async Task<IActionResult> Register(RegisterVM registration)
	{
		_logger.LogInformation("Registration attempt for email: {Email}", registration.Email);

		var types = await _bloodTypeService.GetBloodTypes();

		ViewBag.BloodTypes = new SelectList(types, "Id", "Type", registration.BloodTypeId);

		if (!ModelState.IsValid)
		{
			_logger.LogWarning("ModelState is invalid during registration for email: {Email}", registration.Email);
			return View(registration);
		}

		var returnUrl = Url.Content("~/");
		var authenticateRequestVM = await _authService.Register(registration);

		if (authenticateRequestVM.Succeeded)
		{
			if (authenticateRequestVM.EmailConfirmed.HasValue && !authenticateRequestVM.EmailConfirmed.Value)
			{
				_logger.LogInformation("Registration successful, but email not confirmed for user: {Email}", registration.Email);
				return RedirectToAction("EmailNotConfirmed", new { email = registration.Email });
			}

			_logger.LogInformation("Registration successful for user: {Email}", registration.Email);
			return LocalRedirect(returnUrl);
		}

		_logger.LogWarning("Registration failed for user: {Email}", registration.Email);
		ModelState.AddModelError("", "Registration Attempt Failed. Please try again.");
		return View(registration);
	}


	[HttpPost]
	public async Task<IActionResult> Logout(string returnUrl)
	{
		returnUrl ??= Url.Content("~/");
		await _authService.Logout();
		return LocalRedirect(returnUrl);
	}
}