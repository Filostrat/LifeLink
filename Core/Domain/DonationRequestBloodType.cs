using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain;

public class DonationRequestBloodType
{
	public int Id { get; set; }

	public int DonationRequestId { get; set; }
	public DonationRequest DonationRequest { get; set; }

	public int BloodTypeId { get; set; }
	public BloodType BloodType { get; set; }
}