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
		CreateMap<DonorVM, DonorResponseDTO>().ReverseMap()
			.ForMember(dest => dest.LastDonation, opt => opt.MapFrom(src => src.LastDonation.Value.LocalDateTime));

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
	}
}