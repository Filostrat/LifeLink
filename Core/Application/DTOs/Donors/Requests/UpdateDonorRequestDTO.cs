using Application.DTOs.Notifications;
using Domain;

namespace Application.DTOs.Donors.Requests;

public class UpdateDonorRequestDTO
{
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public double? Height { get; set; }
	public double? Weight { get; set; }
	public string? Gender { get; set; }
	public int? BloodTypeId { get; set; }
	public DateTime? LastDonation { get; set; }
	public string? AddressLine { get; set; }
	public string? City { get; set; }
	public string? PostalCode { get; set; }

	public double? Latitude { get; set; }
	public double? Longitude { get; set; }

	public ICollection<NotificationChannelEnum> PreferredChannels { get; set; } = new List<NotificationChannelEnum>();
}