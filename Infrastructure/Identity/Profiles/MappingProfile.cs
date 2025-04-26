using AutoMapper;

using Domain;
using Microsoft.AspNetCore.Identity;


namespace Identity.Profiles;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<IdentityUser, User>();
	}
}