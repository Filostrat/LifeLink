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

		AddCompatibility("O-", "O-");
		AddCompatibility("O-", "O+");
		AddCompatibility("O-", "A-");
		AddCompatibility("O-", "A+");
		AddCompatibility("O-", "B-");
		AddCompatibility("O-", "B+");
		AddCompatibility("O-", "AB-");
		AddCompatibility("O-", "AB+");
		AddCompatibility("O+", "O+");
		AddCompatibility("O+", "A+");
		AddCompatibility("O+", "B+");
		AddCompatibility("O+", "AB+");
		AddCompatibility("A-", "A-");
		AddCompatibility("A-", "A+");
		AddCompatibility("A-", "AB-");
		AddCompatibility("A-", "AB+");
		AddCompatibility("A+", "A+");
		AddCompatibility("A+", "AB+");
		AddCompatibility("B-", "B-");
		AddCompatibility("B-", "B+");
		AddCompatibility("B-", "AB-");
		AddCompatibility("B-", "AB+");
		AddCompatibility("B+", "B+");
		AddCompatibility("B+", "AB+");
		AddCompatibility("AB-", "AB-");
		AddCompatibility("AB-", "AB+");
		AddCompatibility("AB+", "AB+");

		builder.HasData(compatibilityList);
	}
}