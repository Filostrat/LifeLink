using Domain;


namespace Persistence.Constants;

public static class BloodTypeData
{
	public static readonly List<BloodType> BloodTypes = new()
	{
		new BloodType { Id = 1, Type = "O-" },
		new BloodType { Id = 2, Type = "O+" },
		new BloodType { Id = 3, Type = "A-" },
		new BloodType { Id = 4, Type = "A+" },
		new BloodType { Id = 5, Type = "B-" },
		new BloodType { Id = 6, Type = "B+" },
		new BloodType { Id = 7, Type = "AB-" },
		new BloodType { Id = 8, Type = "AB+" }
	};
}