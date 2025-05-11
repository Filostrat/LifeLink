using Application.Contracts.Infrastructure;
using Application.Contracts.Notifications;
using Application.Contracts.Persistence;
using Application.DTOs.Notifications;
using Application.Features.DonationRequests.Requests.Commands;
using AutoMapper;

using Domain;

using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetTopologySuite.Geometries;


namespace Application.Features.DonationRequests.Handlers.Commands;

public class CreateDonationRequestCommandHandler: IRequestHandler<CreateDonationRequestCommand, Unit>
{
	private readonly IEnumerable<IDonationRequestNotificationChannelService> _channels;
	private readonly ILogger<CreateDonationRequestCommandHandler> _logger;
	private readonly IDonationRequestRepository _donationRequestRepository;
	private readonly IEmailTemplateBuilder _emailTemplateBuilder;
	private readonly IDonorRepository _donorRepository;
	private readonly IMapper _mapper;

	public CreateDonationRequestCommandHandler(IEnumerable<IDonationRequestNotificationChannelService> channels,
											   IDonationRequestRepository donationRequestRepository,
											   IEmailTemplateBuilder emailTemplateBuilder,
											   IDonorRepository donorRepository,
											   IMapper mapper,
											   ILogger<CreateDonationRequestCommandHandler> logger)
	{
		_donationRequestRepository = donationRequestRepository;
		_emailTemplateBuilder = emailTemplateBuilder;
		_donorRepository = donorRepository;
		_mapper = mapper;
		_logger = logger;
		_channels = channels;
	}

	public async Task<Unit> Handle(CreateDonationRequestCommand request,CancellationToken cancellationToken)
	{
		var donationRequest = _mapper.Map<DonationRequest>(request);
		await _donationRequestRepository.AddAsync(donationRequest, cancellationToken);

		var donors = await _donorRepository
			.GetDonorsByBloodTypeAndLocationAsync(
				request.BloodTypeId,
				new Point(request.Longitude, request.Latitude) { SRID = 4326 },
				cancellationToken);

		if (donors is null)
			return Unit.Value;

		foreach (var donor in donors)
		{
			foreach (var channel in donor.Preference.Channels)
			{
				var service = _channels
					.FirstOrDefault(s => s.ChannelName == channel.Channel);

				if (service is null)
				{
					_logger.LogWarning(
						"No channel service for {Channel} (user {Email})",
						channel.Channel, donor.Email);
					continue;
				}

				var donationNotificationInfoDTO = new DonationNotificationInfoDTO
				{
					Latitude = request.Latitude,
					Longitude = request.Longitude,
					City = request.City,
					Email = donor.Email,
					FirstName = donor.FirstName,
					LastName = donor.LastName
				};

				await service.PublishAsync(donationNotificationInfoDTO, cancellationToken);

				donationRequest.Notifications.Add(new DonationRequestNotification
				{
					DonorId = donor.Id,
					Email = donor.Email,
					Channel = channel.Channel,
					SentAt = DateTime.UtcNow,
					DonationRequestId = donationRequest.Id
				});
			}
		}

		await _donationRequestRepository.UpdateAsync(donationRequest, cancellationToken);
		return Unit.Value;
	}
}