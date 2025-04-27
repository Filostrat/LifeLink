using Application.Contracts.Persistence;
using Application.DTOs.DonationRequest.Requests;
using Application.Features.DonationRequests.Requests.Queries;

using AutoMapper;

using MediatR;


namespace Application.Features.DonationRequests.Handlers.Queries;

public class GetAllDonationRequestsQueryHandler : IRequestHandler<GetAllDonationRequestsQuery, List<DonationRequestDTO>>
{
	private readonly IDonationRequestRepository _repository;
	private readonly IMapper _mapper;

	public GetAllDonationRequestsQueryHandler(IDonationRequestRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<List<DonationRequestDTO>> Handle(GetAllDonationRequestsQuery request, CancellationToken cancellationToken)
	{
		var entities = await _repository.GetAllWithNotificationsAsync(request.AdminId);
		return _mapper.Map<List<DonationRequestDTO>>(entities);
	}
}