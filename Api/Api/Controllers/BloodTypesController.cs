using Application.DTOs.BloodType.Requests;
using Application.Features.BloodTypes.Requests.Queries;
using MediatR;

using Microsoft.AspNetCore.Mvc;
using System.Threading;


namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BloodTypesController : ControllerBase
{
	private readonly IMediator _mediator;

	public BloodTypesController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("blood_types")]
	public async Task<ActionResult<List<BloodTypeDTO>>> Get(CancellationToken cancellationToken)
	{
		var result = await _mediator.Send(new GetAllBloodTypesQuery(), cancellationToken);
		return Ok(result);
	}
}
