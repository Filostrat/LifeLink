using Application.DTOs.Notifications;
using Domain;

namespace Application.Contracts.Notifications;

public interface IDonationRequestNotificationChannelService
{
	NotificationChannelEnum ChannelName { get; }

	Task PublishAsync(DonationNotificationInfoDTO message, CancellationToken cancellationToken);
}