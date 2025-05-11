using Application.Contracts.Infrastructure;
using Application.Contracts.Notifications;
using Application.Contracts.Persistence;
using Application.Features.DonationRequests.Requests.Commands;
using AutoMapper;

using Domain;

using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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

	public class CreateDonationRequestCommandHandler
	: IRequestHandler<CreateDonationRequestCommand, Unit>
	{
		private readonly IEnumerable<INotificationChannelService> _channels;
		private readonly ILogger<CreateDonationRequestCommandHandler> _logger;
		private readonly IDonationRequestRepository _donationRequestRepository;
		private readonly IEmailTemplateBuilder _emailTemplateBuilder;
		private readonly IDonorRepository _donorRepository;
		private readonly IMapper _mapper;
		private readonly IOptions<KafkaSettings> _kafkaSettings;

		public CreateDonationRequestCommandHandler(
			IDonationRequestRepository donationRequestRepository,
			IEmailTemplateBuilder emailTemplateBuilder,
			IDonorRepository donorRepository,
			IEnumerable<INotificationChannelService> channels,
			IMapper mapper,
			IOptions<KafkaSettings> kafkaSettings,
			ILogger<CreateDonationRequestCommandHandler> logger)
		{
			_donationRequestRepository = donationRequestRepository;
			_emailTemplateBuilder = emailTemplateBuilder;
			_donorRepository = donorRepository;
			_channels = channels;
			_mapper = mapper;
			_kafkaSettings = kafkaSettings;
			_logger = logger;
		}

		public async Task<Unit> Handle(
			CreateDonationRequestCommand request,
			CancellationToken cancellationToken)
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
				var emailDto = await _emailTemplateBuilder
					.CreateDonationRequestEmail(
						donor.Email,
						request.City,
						request.Latitude,
						request.Longitude);

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

					await service.PublishAsync(emailDto, cancellationToken);

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
}