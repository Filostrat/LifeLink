using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
	public void Configure(EntityTypeBuilder<IdentityRole> builder)
	{
		builder.HasData(
			new IdentityRole
			{
				Id = "EFA5BCA3-14A3-41B0-9345-3067E7AFA050",
				Name = "Administrator",
				NormalizedName = "ADMINISTRATOR",
			},
			new IdentityRole
			{
				Id = "35E286E8-FFE7-44C8-92AB-E27C1F704FDC",
				Name = "User",
				NormalizedName = "USER",
			}
		);
	}
}