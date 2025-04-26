namespace Application.DTOs.DonationRequest.Requests;

public class CreateDonationRequestDTO
{
	public int BloodTypeId { get; set; }
	public string City { get; set; }
	public double Latitude { get; set; }
	public double Longitude { get; set; }
	public double RadiusInMeters { get; set; }
}