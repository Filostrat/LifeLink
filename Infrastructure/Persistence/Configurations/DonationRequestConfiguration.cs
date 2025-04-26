using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;
public class DonationRequestConfiguration : IEntityTypeConfiguration<DonationRequest>
{
	public void Configure(EntityTypeBuilder<DonationRequest> builder)
	{
		builder.HasKey(bc => bc.Id);
	}
}
