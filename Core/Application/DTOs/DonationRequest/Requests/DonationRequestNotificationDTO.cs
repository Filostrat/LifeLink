namespace Application.DTOs.DonationRequest.Requests;

public class DonationRequestNotificationDTO
{
	public int DonorId { get; set; }
	public string Email { get; set; }
	public DateTime SentAt { get; set; }
}

