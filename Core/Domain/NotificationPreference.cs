namespace Domain;

public class NotificationPreference
{
	public int Id { get; set; }
	public int DonorId { get; set; }
	public Donor Donor { get; set; } = null!;
	public ICollection<NotificationChannel> Channels { get; set; } = new List<NotificationChannel>();
}