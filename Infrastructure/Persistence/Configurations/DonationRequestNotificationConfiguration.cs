using Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Configurations;

public class DonationRequestNotificationConfiguration : IEntityTypeConfiguration<DonationRequestNotification>
{
	public void Configure(EntityTypeBuilder<DonationRequestNotification> builder)
	{
		builder.HasKey(x => x.Id);
		builder.Property(x => x.Email)
			   .IsRequired()
			   .HasMaxLength(200);
		builder.Property(x => x.SentAt)
			   .IsRequired();

		builder.HasOne(n => n.DonationRequest)
			   .WithMany(dr => dr.Notifications)
			   .HasForeignKey(n => n.DonationRequestId);
	}
}