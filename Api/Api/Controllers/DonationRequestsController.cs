using Api.Services.Contracts;
using Application.DTOs.DonationRequest.Requests;
using Application.Features.DonationRequests.Requests.Commands;
using Application.Features.DonationRequests.Requests.Queries;
using AutoMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;


namespace Api.Controllers;

[Authorize(Roles = "Administrator")]
public class DonationRequestsController : Controller
{
	private readonly IHttpContextService _httpContextService;
	private readonly IMediator _mediator;
	private readonly IMapper _mapper;

	public DonationRequestsController(IHttpContextService httpContextService, 
									  IMediator mediator,
									  IMapper mapper)
	{
		_httpContextService = httpContextService;
		_mediator = mediator;
		_mapper = mapper;
	}

	[HttpPost("create_donation_request")]
	public async Task<IActionResult> CreateDonationRequest([FromBody] CreateDonationRequestDTO request, CancellationToken cancellationToken)
	{
		var query = _mapper.Map<CreateDonationRequestCommand> (request);

		query.AdminId = _httpContextService.UserId;

		await _mediator.Send(query, cancellationToken);
		return Ok();
	}

	[HttpGet("all_donation_requests")]
	public async Task<ActionResult<List<DonationRequestDTO>>> GetAllDonationRequests(CancellationToken cancellationToken)
	{
		var query = new GetAllDonationRequestsQuery
		{
			AdminId = _httpContextService.UserId
		};

		var result = await _mediator.Send(query, cancellationToken);
		return Ok(result);
	}

	[HttpGet("donation_request/{id}")]
	public async Task<ActionResult<DonationRequestDTO>> GetDonationRequestById(int id, CancellationToken cancellationToken)
	{
		var query = new GetDonationRequestByIdQuery(id);

		var result = await _mediator.Send(query, cancellationToken);

		return Ok(result);
	}
}