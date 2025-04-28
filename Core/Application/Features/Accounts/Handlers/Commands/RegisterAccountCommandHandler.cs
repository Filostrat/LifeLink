using Application.Contracts.Identity;
using Application.Contracts.Persistence;
using Application.DTOs.Account.Responses;
using Application.Exceptions;
using Application.Features.Accounts.Requests.Commands;
using AutoMapper;
using MediatR;


namespace Application.Features.Accounts.Handlers.Commands;

public class RegisterAccountCommandHandler : IRequestHandler<RegisterAccountCommand, LoginAccountResponseDTO>
{
	private readonly IAuthenticationService _authenticationService;
	private readonly IDonorRepository _donorRepository;
	private readonly IJwtService _jwtService;
	private readonly IMapper _mapper;

	public RegisterAccountCommandHandler(IAuthenticationService authenticationService,
										 IDonorRepository donorRepository,
										 IJwtService jwtService,
										 IMapper mapper)
	{
		_authenticationService = authenticationService;
		_donorRepository = donorRepository;
		_jwtService = jwtService;
		_mapper = mapper;
	}

	public async Task<LoginAccountResponseDTO> Handle(RegisterAccountCommand request, CancellationToken cancellationToken)
	{
		var newUser = await _authenticationService
			.RegisterAsync(
				request.UserName,
				request.Email,
				request.Password
			) ?? throw new UserCreationFailedException();

		var token = await _jwtService.GenerateToken(newUser);

		var dto = _mapper.Map<LoginAccountResponseDTO>(newUser);
		dto.Token = token;

		var donor = new Domain.Donor()
		{
			FirstName = request.FirstName,
			LastName = request.LastName,
			BloodTypeId = request.BloodTypeId,
			Email = request.Email.ToUpper(),
		};

		await _donorRepository.AddAsync(donor, cancellationToken);

		return dto;
	}
}