using Application.Contracts.Infrastructure;
using Application.Contracts.Persistence;
using Application.Features.DonationRequests.Requests.Commands;
using AutoMapper;

using Domain;

using MediatR;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;


namespace Application.Features.DonationRequests.Handlers.Commands;

public class CreateDonationRequestCommandHandler : IRequestHandler<CreateDonationRequestCommand, Unit>
{
	private readonly ILogger<CreateDonationRequestCommandHandler> _logger;
	private readonly IDonationRequestRepository _donationRequestRepository;
	private readonly IEmailTemplateBuilder _emailTemplateBuilder;
	private readonly IDonorRepository _donorRepository;
	private readonly IMessageBus _messageBus;
	private readonly IMapper _mapper;

	public CreateDonationRequestCommandHandler(IDonationRequestRepository donationRequestRepository,
											   IEmailTemplateBuilder emailTemplateBuilder,
											   IDonorRepository donorRepository,
											   IMessageBus messageBus,
											   IMapper mapper)
	{
		_donationRequestRepository = donationRequestRepository;
		_emailTemplateBuilder = emailTemplateBuilder;
		_donorRepository = donorRepository;
		_messageBus = messageBus;
		_mapper = mapper;
	}

	public async Task<Unit> Handle(CreateDonationRequestCommand request, CancellationToken cancellationToken)
	{
		var donationRequest = _mapper.Map<DonationRequest>(request);

		await _donationRequestRepository.AddAsync(donationRequest, cancellationToken);

		var donors = await _donorRepository.GetDonorsByBloodTypeAndLocationAsync(
			request.BloodTypeId,
			new Point(request.Longitude, request.Latitude) { SRID = 4326 }, cancellationToken);

		if (donors != null)
		{
			foreach (var item in donors)
			{
				var emailMessage = await _emailTemplateBuilder.CreateDonationRequestEmail(
					item.Email, request.City, request.Latitude, request.Longitude);

				await _messageBus.PublishAsync(emailMessage, cancellationToken);

				donationRequest.Notifications.Add(new DonationRequestNotification
				{
					DonorId = item.Id,
					Email = item.Email,
					SentAt = DateTime.UtcNow,
					DonationRequestId = donationRequest.Id
				});
			}

			await _donationRequestRepository.UpdateAsync(donationRequest, cancellationToken);
		}

		return Unit.Value;
	}
}