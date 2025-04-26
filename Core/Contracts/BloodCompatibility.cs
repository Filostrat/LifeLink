namespace Domain;

public class BloodCompatibility
{
	public int Id { get; set; }

	public int FromBloodTypeId { get; set; }
	public BloodType FromBloodType { get; set; }

	public int ToBloodTypeId { get; set; }
	public BloodType ToBloodType { get; set; }
}