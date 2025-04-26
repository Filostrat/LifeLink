using NetTopologySuite.Geometries;

namespace Domain;

public class Donor
{
	public int Id { get; set; }

	public string Email { get; set; } = null!;

	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public double? Height { get; set; }
	public double? Weight { get; set; }
	public string? Gender { get; set; } 

	public int BloodTypeId { get; set; }
	public BloodType BloodType { get; set; } = null!;

	public DateTime? LastDonation { get; set; }

	public string? AddressLine { get; set; }
	public string? City { get; set; }
	public string? PostalCode { get; set; }
	public Point? Location { get; set; }
}
