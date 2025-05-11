using Domain;

using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;


namespace Persistence.Configurations;

public class NotificationPreferenceConfiguration : IEntityTypeConfiguration<NotificationPreference>
{
	public void Configure(EntityTypeBuilder<NotificationPreference> builder)
	{
		builder.HasKey(p => p.DonorId);

		builder.HasMany(p => p.Channels)
			.WithOne(c => c.Preference)
			.HasForeignKey(c => c.NotificationPreferenceId);
	}
}