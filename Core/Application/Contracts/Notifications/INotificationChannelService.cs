using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Notifications;

public interface INotificationChannelService
{
	NotificationChannelEnum ChannelName { get; }

	Task PublishAsync<T>(T message, CancellationToken cancellationToken);
}