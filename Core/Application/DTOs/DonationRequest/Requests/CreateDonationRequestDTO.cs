namespace Application.DTOs.DonationRequest.Requests;

public class CreateDonationRequestDTO
{
	public List<int> BloodTypeId { get; set; }
	public string City { get; set; }
	public string Message { get; set; }
	public double Latitude { get; set; }
	public double Longitude { get; set; }
}