using Api.Services.Contracts;

using Application.DTOs.Donors.Requests;
using Application.DTOs.Donors.Responses;
using Application.Features.Donors.Requests.Commands;
using Application.Features.Donors.Requests.Queries;
using AutoMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;



namespace Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class DonorsController : ControllerBase
{
	private readonly IHttpContextService _httpContextService;
	private readonly ILogger<AccountController> _logger;
	private readonly IMediator _mediator;
	private readonly IMapper _mapper;

	public DonorsController(IHttpContextService httpContextService,
							 ILogger<AccountController> logger,
							 IMediator mediator,
							 IMapper mapper)
	{
		_httpContextService = httpContextService;
		_mediator = mediator;
		_mapper = mapper;
		_logger = logger;
	}

	[Authorize]
	[HttpGet("donor_information")]
	public async Task<ActionResult<DonorResponseDTO>> GetInformationAboutMe(CancellationToken cancellationToken)
	{
		var email = _httpContextService.Email;

		var donorDto = await _mediator.Send(new GetCurrentDonorQuery() { Email = email}, cancellationToken);

		return Ok(donorDto);
	}

	[Authorize]
	[HttpPut("update_donor_info")]
	public async Task<ActionResult<DonorResponseDTO>> UpdateInformationAboutMe([FromBody] UpdateDonorRequestDTO request,CancellationToken cancellationToken)
	{
		var email = _httpContextService.Email;

		var command = _mapper.Map<UpdateCurrentDonorCommand>(request);

		command.Email = email;

		var updatedDto = await _mediator.Send(command, cancellationToken);
		return Ok(updatedDto);
	}
}