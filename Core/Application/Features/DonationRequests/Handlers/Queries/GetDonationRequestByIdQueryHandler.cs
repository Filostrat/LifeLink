using Application.Contracts.Persistence;
using Application.DTOs.DonationRequest.Requests;
using Application.Exceptions;
using Application.Features.DonationRequests.Requests.Queries;

using AutoMapper;

using MediatR;


namespace Application.Features.DonationRequests.Handlers.Queries;

public class GetDonationRequestByIdQueryHandler : IRequestHandler<GetDonationRequestByIdQuery, DonationRequestDTO>
{
	private readonly IDonationRequestRepository _repository;
	private readonly IMapper _mapper;

	public GetDonationRequestByIdQueryHandler(IDonationRequestRepository repository,
											  IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<DonationRequestDTO> Handle(GetDonationRequestByIdQuery request,CancellationToken cancellationToken)
	{
		var entity = await _repository.GetWithIncludesAsync(request.Id, cancellationToken) ?? 
			throw new DonationRequestNotFoundException(request.Id);

		return _mapper.Map<DonationRequestDTO>(entity);
	}
}