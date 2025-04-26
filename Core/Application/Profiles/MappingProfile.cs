using Application.DTOs.Account.Requests;
using Application.DTOs.Account.Responses;
using Application.DTOs.BloodType.Requests;
using Application.DTOs.DonationRequest.Requests;
using Application.DTOs.Donors.Requests;
using Application.DTOs.Donors.Responses;
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
		CreateMap<BloodType, BloodTypeDto>();
		CreateMap<Donor, DonorResponseDTO>();
		CreateMap<UpdateCurrentDonorCommand, UpdateDonorRequestDTO>().ReverseMap();


		CreateMap<UpdateCurrentDonorCommand, Donor>()
			.ForMember(dest => dest.Location, opt => opt.Ignore());

		CreateMap<User, LoginAccountResponseDTO>()
			.ForMember(dest => dest.Token, opt => opt.Ignore());

		CreateMap<CreateDonationRequestDTO, CreateDonationRequestCommand>()
			.ForMember(dest => dest.AdminId, opt => opt.Ignore());

		CreateMap<CreateDonationRequestCommand, DonationRequest>()
			.ForMember(dest => dest.CreationDateTime, opt => opt.MapFrom(_ => DateTime.UtcNow));
	}
}
