using AutoMapper;

using UI.Models.Authentication;
using UI.Models.DonationRequest;
using UI.Models.Donors;
using UI.Services.Base;


namespace UI.Profiles;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<RegisterVM, RegisterAccountRequestDTO>().ReverseMap();

		CreateMap<UpdateDonorRequestDTO, DonorVM>().ReverseMap();
		CreateMap<CreateDonationRequestDTO, CreateDonationRequestVM>().ReverseMap();

		CreateMap<BloodTypeDTO, DonationRequestVM>().ReverseMap();

		CreateMap<DonationRequestNotificationDTO, DonationRequestNotificationVM>()
			.ForMember(dest => dest.SentAt, opt => opt.MapFrom(src => src.SentAt.Value.UtcDateTime))
			.ReverseMap()
			.ForMember(dest => dest.SentAt, opt => opt.MapFrom(src => new DateTimeOffset(src.SentAt)));

		CreateMap<DonationRequestDTO, DonationRequestVM>()
			.ForMember(dest => dest.CreationDateTime, opt => opt.MapFrom(src => src.CreationDateTime.Value.UtcDateTime))
			.ReverseMap()
			.ForMember(dest => dest.CreationDateTime, opt => opt.MapFrom(src => new DateTimeOffset(src.CreationDateTime)));

		CreateMap<DonorVM, UpdateDonorRequestDTO>()
			.ForMember(dest => dest.PreferredChannels,
					   opt => opt.MapFrom(src
						   => src.PreferredChannels
								 .Select(i => (NotificationChannelEnum)i)
								 .ToArray()));

		CreateMap<DonorResponseDTO, DonorVM>()
			.ForMember(
				dest => dest.LastDonation,
				opt => opt.MapFrom(src =>
							src.LastDonation.HasValue
								? src.LastDonation.Value.LocalDateTime
								: (DateTime?)null
				)
			)
			.ForMember(
				dest => dest.PreferredChannels,
				opt => opt.MapFrom(src =>
							src.PreferredChannels.Select(c => (int)c).ToArray()
				)
			)
			.ReverseMap()
			.ForMember(
				dest => dest.LastDonation,
				opt => opt.MapFrom(src => new DateTimeOffset(src.LastDonation.Value))
			)
			.ForMember(
				dest => dest.PreferredChannels,
				opt => opt.MapFrom(src =>
							src.PreferredChannels.Select(i => (NotificationChannelEnum)i).ToArray()
				)
			);
	}
}