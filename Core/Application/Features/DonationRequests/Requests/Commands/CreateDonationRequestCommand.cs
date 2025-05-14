using MediatR;


namespace Application.Features.DonationRequests.Requests.Commands;

public class CreateDonationRequestCommand : IRequest<Unit>
{
	public string AdminId { get; set; }
	public List<int> BloodTypeId { get; set; }
	public string City { get; set; }
	public string Message { get; set; }
	public double Latitude { get; set; }
	public double Longitude { get; set; }
}
