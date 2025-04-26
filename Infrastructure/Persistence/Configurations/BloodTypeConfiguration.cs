using Domain;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Constants;


namespace Persistence.Configurations;

public class BloodTypeConfiguration : IEntityTypeConfiguration<BloodType>
{
	public void Configure(EntityTypeBuilder<BloodType> builder)
	{
		builder.HasData(BloodTypeData.BloodTypes);
	}
}

