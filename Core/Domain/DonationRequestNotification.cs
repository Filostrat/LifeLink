namespace Domain;

public class DonationRequestNotification
{
	public int Id { get; set; }
	public int DonationRequestId { get; set; }
	public DonationRequest DonationRequest { get; set; }
	public NotificationChannelEnum Channel { get; set; }
	public int DonorId { get; set; }
	public string Email { get; set; }
	public DateTime SentAt { get; set; }
}