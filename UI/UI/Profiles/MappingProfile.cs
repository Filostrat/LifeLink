using AutoMapper;
using UI.Models.Authentication;
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
	}
}