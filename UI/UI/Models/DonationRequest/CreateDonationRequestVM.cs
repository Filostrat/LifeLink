using System.ComponentModel.DataAnnotations;

namespace UI.Models.DonationRequest;

public class CreateDonationRequestVM
{
	public List<int> BloodTypeId { get; set; } = new List<int>();

	[Required(ErrorMessage = "Місто обов'язкове для введення")]
	public string City { get; set; }

	public string Message { get; set; }
	public double Latitude { get; set; }
	public double Longitude { get; set; }
}