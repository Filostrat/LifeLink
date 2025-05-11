using Application.DTOs.Account.Requests;
using Application.DTOs.Account.Responses;
using Application.DTOs.BloodType.Requests;
using Application.DTOs.DonationRequest.Requests;
using Application.DTOs.Donors.Requests;
using Application.DTOs.Donors.Responses;
using Application.DTOs.Notifications;
using Application.Features.Accounts.Requests.Commands;
using Application.Features.DonationRequests.Requests.Commands;
using Application.Features.Donors.Requests.Commands;

using AutoMapper;

using Domain;


namespace Application.Profiles;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<LoginAccountRequestDTO, LoginAccountCommand>();
		CreateMap<RegisterAccountRequestDTO, RegisterAccountCommand>();
		CreateMap<BloodType, BloodTypeDTO>();

		CreateMap<Donor, DonorResponseDTO>()
			.ForMember(dest => dest.Channels, opt => opt.MapFrom(src =>
						   src.Preference != null
						   ? src.Preference.Channels.Select(c => c.Channel)
						   : Enumerable.Empty<NotificationChannelEnum>()));

		CreateMap<UpdateCurrentDonorCommand, UpdateDonorRequestDTO>()
			.ForMember(d => d.PreferredChannels,
					   opt => opt.MapFrom(src => src.Channels ?? Enumerable.Empty<NotificationChannelEnum>()))
			.ReverseMap()
			.ForMember(d => d.Channels,
					   opt => opt.MapFrom(src => src.PreferredChannels ?? new List<NotificationChannelEnum>()));

		CreateMap<NotificationChannelEnum, NotificationPreferenceDTO>()
			.ConstructUsing(src => new NotificationPreferenceDTO { Channel = src });
		CreateMap<NotificationPreferenceDTO, NotificationChannelEnum>()
			.ConvertUsing(dto => dto.Channel);


		CreateMap<UpdateCurrentDonorCommand, Donor>()
			.ForMember(dest => dest.Location, opt => opt.Ignore());

		CreateMap<User, LoginAccountResponseDTO>()
			.ForMember(dest => dest.Token, opt => opt.Ignore());

		CreateMap<CreateDonationRequestDTO, CreateDonationRequestCommand>()
			.ForMember(dest => dest.AdminId, opt => opt.Ignore());

		CreateMap<CreateDonationRequestCommand, DonationRequest>()
			.ForMember(dest => dest.CreationDateTime, opt => opt.MapFrom(_ => DateTime.UtcNow));

		CreateMap<DonationRequest, DonationRequestDTO>()
			.ForMember(dest => dest.BloodTypeName, opt => opt.MapFrom(src => src.BloodType.Type));

		CreateMap<DonationRequestNotification, DonationRequestNotificationDTO>();
	}
}
