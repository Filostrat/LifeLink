using Application.DTOs.DonationRequest.Requests;

using MediatR;


namespace Application.Features.DonationRequests.Requests.Queries;

public class GetDonationRequestByIdQuery : IRequest<DonationRequestDTO>
{
	public int Id { get; }
	public GetDonationRequestByIdQuery(int id) => Id = id;
}
