using Application.Contracts.Identity;
using Application.DTOs.Account.Responses;
using Application.Features.Accounts.Requests.Commands;
using AutoMapper;

using MediatR;


namespace Application.Features.Accounts.Handlers.Commands
{
	public class LoginAccountCommandHandler : IRequestHandler<LoginAccountCommand, LoginAccountResponseDTO>
	{
		private readonly IAuthenticationService _authenticationService;
		private readonly IJwtService _jwtService;
		private readonly IMapper _mapper;

		public LoginAccountCommandHandler(IAuthenticationService authenticationService,
										  IJwtService jwtService,
										  IMapper mapper)
		{
			_authenticationService = authenticationService;
			_jwtService = jwtService;
			_mapper = mapper;
		}

		public async Task<LoginAccountResponseDTO> Handle(LoginAccountCommand request, CancellationToken cancellationToken)
		{
			var user = await _authenticationService.AuthenticateAsync(request.Email, request.Password);

			var token = await _jwtService.GenerateToken(user);

			var dto = _mapper.Map<LoginAccountResponseDTO>(user);
			dto.Token = token;
			return dto;
		}
	}
}
