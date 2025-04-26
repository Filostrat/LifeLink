using Application.Contracts.Infrastructure;
using Application.Contracts.Persistence;
using Application.Features.DonationRequests.Requests.Commands;
using AutoMapper;

using Domain;

using MediatR;

using NetTopologySuite.Geometries;


namespace Application.Features.DonationRequests.Handlers.Commands;

public class CreateDonationRequestCommandHandler : IRequestHandler<CreateDonationRequestCommand, Unit>
{
	private readonly IDonationRequestRepository _donationRequestRepository;
	private readonly IEmailTemplateBuilder _emailTemplateBuilder;
	private readonly IDonorRepository _repository;
	private readonly IMapper _mapper;

	public CreateDonationRequestCommandHandler(IDonationRequestRepository donationRequestRepository,
											   IEmailTemplateBuilder emailTemplateBuilder,
											   IDonorRepository repository,
											   IMapper mapper)
	{
		_donationRequestRepository = donationRequestRepository;
		_emailTemplateBuilder = emailTemplateBuilder;
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<Unit> Handle(CreateDonationRequestCommand request, CancellationToken cancellationToken)
	{

		var donationRequest = _mapper.Map<DonationRequest>(request);

		await _donationRequestRepository.AddAsync(donationRequest);

		var donors = await _repository.GetDonorsByBloodTypeAndLocationAsync(
			request.BloodTypeId,
			new Point(request.Longitude, request.Latitude) { SRID = 4326 },
			request.RadiusInMeters);

		if (donors != null)
		{
			foreach (var item in donors)
			{
				await _emailTemplateBuilder.CreateDonationRequestEmailTemplate(
					item.Email, item.City, request.Latitude, request.Longitude, cancellationToken);
			}
		}

		return Unit.Value;
	}
}