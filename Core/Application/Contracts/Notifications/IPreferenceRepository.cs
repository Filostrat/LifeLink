using Application.Contracts.Persistence;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Notifications;

public interface IPreferenceRepository : IGenericRepository<NotificationPreference>
{
	Task<NotificationPreference> GetForUserAsync(string userId, CancellationToken cancellationToken);
}