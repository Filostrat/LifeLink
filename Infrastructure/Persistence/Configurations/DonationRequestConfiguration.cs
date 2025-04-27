using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class DonationRequestConfiguration : IEntityTypeConfiguration<DonationRequest>
{
	public void Configure(EntityTypeBuilder<DonationRequest> builder)
	{
		builder.HasKey(dr => dr.Id);
		builder.HasOne(dr => dr.BloodType)
			   .WithMany()
			   .HasForeignKey(dr => dr.BloodTypeId);
	}
}
