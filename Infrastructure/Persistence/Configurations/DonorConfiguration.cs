using Domain;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace Persistence.Configurations;

public class DonorConfiguration : IEntityTypeConfiguration<Donor>
{
	public void Configure(EntityTypeBuilder<Donor> builder)
	{
		builder.HasKey(x => x.Id);

		builder.Property(x => x.Email)
			   .IsRequired()
			   .HasMaxLength(200);

		builder.HasIndex(x => x.Email)
			   .IsUnique();


		builder.HasOne(d => d.BloodType)
			   .WithMany()
			   .HasForeignKey(d => d.BloodTypeId)
			   .IsRequired();
	}
}