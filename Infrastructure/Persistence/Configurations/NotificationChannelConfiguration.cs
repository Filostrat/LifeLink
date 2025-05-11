using Domain;

using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistence.Configurations;

public class NotificationChannelConfiguration : IEntityTypeConfiguration<NotificationChannel>
{
	public void Configure(EntityTypeBuilder<NotificationChannel> builder)
	{
		builder.HasKey(c => c.Id);

		builder.Property(c => c.Channel)
			   .HasConversion<string>()
			   .IsRequired();

		builder.HasOne(c => c.Preference)
			   .WithMany(p => p.Channels)
			   .HasForeignKey(c => c.NotificationPreferenceId)
			   .OnDelete(DeleteBehavior.Cascade);
	}
}