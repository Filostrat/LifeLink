namespace UI.Models.DonationRequest;

public class DonationRequestNotificationVM
{
	public int DonorId { get; set; }
	public string Email { get; set; }
	public DateTime SentAt { get; set; }
}