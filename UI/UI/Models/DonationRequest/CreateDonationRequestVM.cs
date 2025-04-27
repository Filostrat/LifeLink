namespace UI.Models.DonationRequest;

public class CreateDonationRequestVM
{
	public int BloodTypeId { get; set; }
	public string City { get; set; }
	public double Latitude { get; set; }
	public double Longitude { get; set; }
}