using Identity.Configurations;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;


namespace Identity;

public class IdentityDbContext : IdentityDbContext<IdentityUser>
{
	public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.ApplyConfiguration(new RoleConfiguration());
		builder.ApplyConfiguration(new UserConfiguration());
		builder.ApplyConfiguration(new UserRoleConfiguration());
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.ConfigureWarnings(warnings => warnings
			.Log(RelationalEventId.PendingModelChangesWarning));
	}
}
