using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<IdentityUser>
{
	public void Configure(EntityTypeBuilder<IdentityUser> builder)
	{
		var hasher = new PasswordHasher<IdentityUser>();
		builder.HasData(
			 new IdentityUser
			 {
				 Id = "717794D8-FCA0-4C24-84F0-55E3BD54D3C4",
				 Email = "admin@localhost.com",
				 NormalizedEmail = "ADMIN@LOCALHOST.COM",
				 UserName = "admin@localhost.com",
				 NormalizedUserName = "ADMIN@LOCALHOST.COM",
				 PasswordHash = hasher.HashPassword(null, "Qwerty@1234"),
				 EmailConfirmed = true
			 },
			 new IdentityUser
			 {
				 Id = "30577476-8FFD-4898-B374-BEE7F2A6BE7F",
				 Email = "user@localhost.com",
				 NormalizedEmail = "USER@LOCALHOST.COM",
				 UserName = "user@localhost.com",
				 NormalizedUserName = "USER@LOCALHOST.COM",
				 PasswordHash = hasher.HashPassword(null, "Qwerty@1234"),
				 EmailConfirmed = true
			 }
		);
	}
}