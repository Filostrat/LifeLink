using Application.Contracts.Identity;
using Application.DTOs.Account.Responses;
using Application.Exceptions;
using Application.Features.Accounts.Requests.Commands;
using AutoMapper;
using MediatR;

namespace Application.Features.Accounts.Handlers.Commands;

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, LoginAccountResponseDTO>
{
	private readonly IAuthenticationService _authenticationService;
	private readonly IJwtService _jwtService;
	private readonly IMapper _mapper;

	public ConfirmEmailCommandHandler(IAuthenticationService authenticationService,
									  IJwtService jwtService,
									  IMapper mapper)
	{
		_authenticationService = authenticationService;
		_jwtService = jwtService;
		_mapper = mapper;
	}

	public async Task<LoginAccountResponseDTO> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
	{
		await _authenticationService.ConfirmEmailAsync(request.UserId, request.Token);

		var user = await _authenticationService.GetUserByIdAsync(request.UserId);

		var token = await _jwtService.GenerateToken(user);

		var dto = _mapper.Map<LoginAccountResponseDTO>(user);
		dto.Token = token;
		dto.EmailConfirmed = true;

		return dto;
	}
}