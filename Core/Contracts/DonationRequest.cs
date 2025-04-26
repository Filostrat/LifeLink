namespace Domain;

public class DonationRequest
{
	public int Id { get; set; }

	public int BloodTypeId { get; set; }

	public string AdminId { get; set; }
	public double Latitude { get; set; }
	public double Longitude { get; set; }
	public double RadiusInMeters { get; set; }

	public DateTime CreationDateTime { get; set; }
}
