namespace Domain;

public class DonationRequest
{
	public int Id { get; set; }

	public ICollection<DonationRequestBloodType> DonationRequestBloodTypes { get; set; }

	public string AdminId { get; set; }
	public string? City { get; set; }
	public double Latitude { get; set; }
	public double Longitude { get; set; }
	public string? Message { get; set; }
	public double RadiusInMeters { get; set; }
	public DateTime CreationDateTime { get; set; }

	public ICollection<DonationRequestNotification> Notifications { get; set; } = [];
}