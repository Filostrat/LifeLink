using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain;

public class NotificationChannel
{
	public int Id { get; set; }
	public int NotificationPreferenceId { get; set; }
	public NotificationChannelEnum Channel { get; set; }
	public NotificationPreference Preference { get; set; } = null!;
}