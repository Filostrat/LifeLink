using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Notifications;

public interface INotificationStrategy
{
	NotificationChannel Channel { get; }
	Task SendAsync(Donor donor, DonationRequest request, CancellationToken cancellationToken);
}