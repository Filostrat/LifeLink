using Domain;


namespace Persistence.Constants;

public static class BloodTypeData
{
	public static readonly List<BloodType> BloodTypes = new()
	{
		new BloodType { Id = 1, Type = "I (0) Rh−" },
		new BloodType { Id = 2, Type = "I (0) Rh+" },
		new BloodType { Id = 3, Type = "II (A) Rh−" },
		new BloodType { Id = 4, Type = "II (A) Rh+" },
		new BloodType { Id = 5, Type = "III (B) Rh−" },
		new BloodType { Id = 6, Type = "III (B) Rh+" },
		new BloodType { Id = 7, Type = "IV (AB) Rh−" },
		new BloodType { Id = 8, Type = "IV (AB) Rh+" }
	};
}