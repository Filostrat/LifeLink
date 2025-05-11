using Application.Contracts.Persistence;
using Application.DTOs.Donors.Responses;
using Application.Exceptions;
using Application.Features.Donors.Requests.Commands;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;


namespace Application.Features.Donors.Handlers.Commands;

public class UpdateCurrentDonorCommandHandler : IRequestHandler<UpdateCurrentDonorCommand, DonorResponseDTO>
{
	private readonly ILogger<UpdateCurrentDonorCommandHandler> _logger;
	private readonly IDonorRepository _donorRepository;
	private readonly IMapper _mapper;

	public UpdateCurrentDonorCommandHandler(IDonorRepository donorRepository, IMapper mapper, ILogger<UpdateCurrentDonorCommandHandler> logger)
	{
		_donorRepository = donorRepository;
		_mapper = mapper;
		_logger = logger;
	}

	public async Task<DonorResponseDTO> Handle(UpdateCurrentDonorCommand request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("Starting UpdateCurrentDonorCommandHandler for email: {Email}", request.Email);

		var donor = await _donorRepository.GetByEmailAsync(request.Email, cancellationToken);
		if (donor == null)
		{
			_logger.LogWarning("Donor with email {Email} not found.", request.Email);
			throw new DonorNotFoundException(request.Email);
		}

		_mapper.Map(request, donor);

		if (request.Latitude.HasValue && request.Longitude.HasValue)
		{
			donor.Location = new Point(request.Longitude.Value, request.Latitude.Value) { SRID = 4326 };
			_logger.LogInformation("Updated donor location to Latitude: {Latitude}, Longitude: {Longitude}", request.Latitude, request.Longitude);
		}


		donor.Preference ??= new NotificationPreference
			{
				Donor = donor
			};


		if (request.Channels != null)
		{
			donor.Preference.Channels.Clear();

			foreach (var ch in request.Channels.Distinct())
			{
				donor.Preference.Channels.Add(new NotificationChannel
				{
					Channel = ch,
				});
			}
			_logger.LogInformation("Notification channels updated: {Channels}", string.Join(", ", request.Channels));
		}

		await _donorRepository.UpdateAsync(donor, cancellationToken);
		_logger.LogInformation("Donor with email {Email} updated successfully.", request.Email);

		donor = await _donorRepository.GetAsync(donor.Id, cancellationToken);
		if (donor == null)
		{
			_logger.LogError("Donor with id {DonorId} not found after update.", donor.Id);
			throw new DonorNotFoundException(request.Email);
		}

		_logger.LogInformation("Successfully retrieved updated donor with id {DonorId}.", donor.Id);

		return _mapper.Map<DonorResponseDTO>(donor);
	}
}
