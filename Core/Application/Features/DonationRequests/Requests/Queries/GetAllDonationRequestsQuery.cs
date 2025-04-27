using Application.DTOs.DonationRequest.Requests;

using MediatR;


namespace Application.Features.DonationRequests.Requests.Queries;

public class GetAllDonationRequestsQuery : IRequest<List<DonationRequestDTO>>
{
	public string AdminId { get; set; }
}
