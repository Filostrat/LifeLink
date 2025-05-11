using Domain;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Constants;


namespace Persistence.Configurations;

public class BloodCompatibilityConfiguration : IEntityTypeConfiguration<BloodCompatibility>
{
	public void Configure(EntityTypeBuilder<BloodCompatibility> builder)
	{
		builder.HasKey(bc => bc.Id);

		builder.HasOne(bc => bc.FromBloodType)
			.WithMany()
			.HasForeignKey(bc => bc.FromBloodTypeId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(bc => bc.ToBloodType)
			.WithMany()
			.HasForeignKey(bc => bc.ToBloodTypeId)
			.OnDelete(DeleteBehavior.Restrict);

		var compatibilityList = new List<BloodCompatibility>();
		int id = 1;

		void AddCompatibility(string donorType, string recipientType)
		{
			var donorId = BloodTypeData.BloodTypes.First(bt => bt.Type == donorType).Id;
			var recipientId = BloodTypeData.BloodTypes.First(bt => bt.Type == recipientType).Id;

			compatibilityList.Add(new BloodCompatibility
			{
				Id = id++,
				FromBloodTypeId = donorId,
				ToBloodTypeId = recipientId
			});
		}

		AddCompatibility("I (0) Rh−", "I (0) Rh−");
		AddCompatibility("I (0) Rh−", "I (0) Rh+");
		AddCompatibility("I (0) Rh−", "II (A) Rh−");
		AddCompatibility("I (0) Rh−", "II (A) Rh+");
		AddCompatibility("I (0) Rh−", "III (B) Rh−");
		AddCompatibility("I (0) Rh−", "III (B) Rh+");
		AddCompatibility("I (0) Rh−", "IV (AB) Rh−");
		AddCompatibility("I (0) Rh−", "IV (AB) Rh+");

		AddCompatibility("I (0) Rh+", "I (0) Rh+");
		AddCompatibility("I (0) Rh+", "II (A) Rh+");
		AddCompatibility("I (0) Rh+", "III (B) Rh+");
		AddCompatibility("I (0) Rh+", "IV (AB) Rh+");

		AddCompatibility("II (A) Rh−", "II (A) Rh−");
		AddCompatibility("II (A) Rh−", "II (A) Rh+");
		AddCompatibility("II (A) Rh−", "IV (AB) Rh−");
		AddCompatibility("II (A) Rh−", "IV (AB) Rh+");

		AddCompatibility("II (A) Rh+", "II (A) Rh+");
		AddCompatibility("II (A) Rh+", "IV (AB) Rh+");

		AddCompatibility("III (B) Rh−", "III (B) Rh−");
		AddCompatibility("III (B) Rh−", "III (B) Rh+");
		AddCompatibility("III (B) Rh−", "IV (AB) Rh−");
		AddCompatibility("III (B) Rh−", "IV (AB) Rh+");

		AddCompatibility("III (B) Rh+", "III (B) Rh+");
		AddCompatibility("III (B) Rh+", "IV (AB) Rh+");

		AddCompatibility("IV (AB) Rh−", "IV (AB) Rh−");
		AddCompatibility("IV (AB) Rh−", "IV (AB) Rh+");
		AddCompatibility("IV (AB) Rh+", "IV (AB) Rh+");

		builder.HasData(compatibilityList);
	}
}