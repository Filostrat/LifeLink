using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Notifications;

public class DonationNotificationMessageInfo
{
	public string? City { get; set; }
	public string? Message { get; set; }
	public string? Email { get; set; }
	public double Latitude { get; set; }
	public double Longitude { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
}