using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Models;

public class TelegramSubscription
{
	public int Id { get; set; }
	public string Email { get; set; } = null!;
	public long ChatId { get; set; }
	public DateTime SubscribedAt { get; set; }
}
