using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class DonationRequestConfiguration : IEntityTypeConfiguration<DonationRequest>
{
	public void Configure(EntityTypeBuilder<DonationRequest> builder)
	{
		builder.HasKey(dr => dr.Id);

		builder.HasMany(dr => dr.DonationRequestBloodTypes)
			   .WithOne(link => link.DonationRequest)
			   .HasForeignKey(link => link.DonationRequestId)
			   .OnDelete(DeleteBehavior.Cascade);
	}
}
