namespace UI.Models.DonationRequest;

public class DonationRequestVM
{
	public int Id { get; set; }
	public int BloodTypeId { get; set; }
	public string BloodTypeName { get; set; }
	public string AdminId { get; set; }
	public string City { get; set; }
	public double Latitude { get; set; }
	public double Longitude { get; set; }
	public double RadiusInMeters { get; set; }
	public DateTime CreationDateTime { get; set; }
	public List<DonationRequestNotificationVM> Notifications { get; set; } = new();
}