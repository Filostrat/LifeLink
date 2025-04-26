using Application.DTOs.Donors.Responses;
using MediatR;


namespace Application.Features.Donors.Requests.Queries;

public class GetCurrentDonorQuery() : IRequest<DonorResponseDTO>
{
	public string Email { get; set; }
}