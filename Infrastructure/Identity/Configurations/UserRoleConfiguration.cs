using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
	public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
	{
		builder.HasData(
			new IdentityUserRole<string>
			{
				RoleId = "EFA5BCA3-14A3-41B0-9345-3067E7AFA050",
				UserId = "717794D8-FCA0-4C24-84F0-55E3BD54D3C4"
			},
			new IdentityUserRole<string>
			{
				RoleId = "35E286E8-FFE7-44C8-92AB-E27C1F704FDC",
				UserId = "30577476-8FFD-4898-B374-BEE7F2A6BE7F"
			}
		);
	}
}