using Application.Contracts.Persistence;
using Application.DTOs.Donors.Responses;
using Application.Exceptions;
using Application.Features.Donors.Requests.Queries;
using AutoMapper;

using MediatR;


namespace Application.Features.Donors.Handlers.Queries;

public class GetCurrentDonorQueryHandler : IRequestHandler<GetCurrentDonorQuery, DonorResponseDTO>
{
	private readonly IDonorRepository _donorRepository;
	private readonly IMapper _mapper;

	public GetCurrentDonorQueryHandler(IDonorRepository donorRepository, IMapper mapper)
	{
		_donorRepository = donorRepository;
		_mapper = mapper;
	}

	public async Task<DonorResponseDTO> Handle(GetCurrentDonorQuery request, CancellationToken cancellationToken)
	{
		var donor = await _donorRepository.GetByEmailAsync(request.Email, cancellationToken) ?? throw new DonorNotFoundException(request.Email);

		return _mapper.Map<DonorResponseDTO>(donor);
	}
}