using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class DonationRequestBloodTypeConfiguration : IEntityTypeConfiguration<DonationRequestBloodType>
{
	public void Configure(EntityTypeBuilder<DonationRequestBloodType> modelBuilder)
	{

		modelBuilder.HasKey(dr => dr.Id);

		modelBuilder
			.HasOne(x => x.DonationRequest)
			.WithMany(dr => dr.DonationRequestBloodTypes)
			.HasForeignKey(x => x.DonationRequestId);

	}
}