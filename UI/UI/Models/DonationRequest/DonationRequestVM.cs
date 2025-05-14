namespace UI.Models.DonationRequest;

public class DonationRequestVM
{
	public int Id { get; set; }
	public List<int> BloodTypeId { get; set; } = new();
	public List<string> BloodTypeName { get; set; } = new();
	public string AdminId { get; set; }
	public string City { get; set; }
	public string Message { get; set; }
	public double Latitude { get; set; }
	public double Longitude { get; set; }
	public double RadiusInMeters { get; set; }
	public DateTime CreationDateTime { get; set; }
	public List<DonationRequestNotificationVM> Notifications { get; set; } = new();
}