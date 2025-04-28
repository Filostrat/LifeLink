using Application.Contracts.Persistence;
using Application.DTOs.BloodType.Requests;
using Application.Features.BloodTypes.Requests.Queries;
using AutoMapper;

using MediatR;


namespace Application.Features.BloodTypes.Handlers.Queries;

public class GetAllBloodTypesQueryHandler : IRequestHandler<GetAllBloodTypesQuery, List<BloodTypeDTO>>
{
	private readonly IBloodTypeRepository _bloodTypeRepository;
	private readonly IMapper _mapper;

	public GetAllBloodTypesQueryHandler(IBloodTypeRepository bloodTypeRepository, IMapper mapper)
	{
		_bloodTypeRepository = bloodTypeRepository;
		_mapper = mapper;
	}

	public async Task<List<BloodTypeDTO>> Handle(GetAllBloodTypesQuery request, CancellationToken cancellationToken)
	{
		var bloodTypes = await _bloodTypeRepository.GetAllAsync(cancellationToken);
		return _mapper.Map<List<BloodTypeDTO>>(bloodTypes);
	}
}
