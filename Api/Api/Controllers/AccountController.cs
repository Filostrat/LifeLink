using Api.Services.Contracts;

using Application.DTOs.Account.Requests;
using Application.DTOs.Account.Responses;

using Application.Features.Accounts.Requests.Commands;
using AutoMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;


namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
	private readonly IHttpContextService _httpContextService;
	private readonly ILogger<AccountController> _logger;
	private readonly IMediator _mediator;
	private readonly IMapper _mapper;

	public AccountController(IHttpContextService httpContextService, 
							 ILogger<AccountController> logger,							 
							 IMediator mediator,
							 IMapper mapper)
	{
		_httpContextService = httpContextService;
		_mediator = mediator;
		_mapper = mapper;
		_logger = logger;
	}

	[HttpPost("login")]
	public async Task<ActionResult<LoginAccountResponseDTO>> Login([FromBody] LoginAccountRequestDTO request, CancellationToken cancellationToken)
	{
		var command = _mapper.Map<LoginAccountCommand>(request);

		var response = await _mediator.Send(command, cancellationToken);

		return Ok(response);
	}

	[HttpPost("register")]
	public async Task<ActionResult<LoginAccountResponseDTO>> Register(RegisterAccountRequestDTO request, CancellationToken cancellationToken)
	{
		var command = _mapper.Map<RegisterAccountCommand>(request);

		var response = await _mediator.Send(command, cancellationToken);

		return Ok(response);
	}

	[HttpGet("confirm-email")]
	[Authorize]
	public async Task<ActionResult<LoginAccountResponseDTO>> ConfirmEmail([FromQuery] string token,CancellationToken cancellationToken)
	{
		var userId = _httpContextService.UserId;

		var command = new ConfirmEmailCommand(userId, token);

		var response = await _mediator.Send(command, cancellationToken);

		return Ok(response);
	}

	[HttpGet("send-confirmation")]
	[Authorize]
	public async Task<IActionResult> SendConfirmationOnEmail([FromQuery] string baseUrl, CancellationToken cancellationToken)
	{
		var userId = _httpContextService.UserId;

		var command = new SendEmailConfirmationCommand(baseUrl, userId);

		await _mediator.Send(command, cancellationToken);

		return Ok();
	}
}