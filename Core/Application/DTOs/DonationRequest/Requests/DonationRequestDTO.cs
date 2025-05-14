namespace Application.DTOs.DonationRequest.Requests;

public class DonationRequestDTO
{
	public int Id { get; set; }
	public List<int> BloodTypeId { get; set; }
	public List<string> BloodTypeName { get; set; }
	public string AdminId { get; set; }
	public string City { get; set; }
	public string Message { get; set; }
	public double Latitude { get; set; }
	public double Longitude { get; set; }
	public double RadiusInMeters { get; set; }
	public DateTime CreationDateTime { get; set; }
	public List<DonationRequestNotificationDTO> Notifications { get; set; }
}
