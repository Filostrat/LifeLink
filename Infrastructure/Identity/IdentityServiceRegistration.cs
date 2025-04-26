using Application.Contracts.Identity;
using Domain.Settings;
using Identity.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using System.Reflection;
using System.Text;


namespace Identity;

public static class IdentityServicesRegistration
{
	public static IServiceCollection ConfigureIdentityServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

		services.AddAutoMapper(Assembly.GetExecutingAssembly());

		services.AddDbContext<IdentityDbContext>(options =>
			options.UseSqlServer(configuration.GetConnectionString("LifeLinkConnectionString"),
			b => b.MigrationsAssembly(typeof(IdentityDbContext).Assembly.FullName)));

		services.AddIdentity<IdentityUser, IdentityRole>()
			.AddEntityFrameworkStores<IdentityDbContext>().AddDefaultTokenProviders();

		services.AddTransient<IAuthenticationService, AuthenticationService>();
		services.AddTransient<IJwtService, JwtService>();

		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		})
			.AddJwtBearer(o =>
			{
				o.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero,
					ValidIssuer = configuration["JwtSettings:Issuer"],
					ValidAudience = configuration["JwtSettings:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]))
				};
			});

		return services;
	}

	public static void ApplyIdentityMigrations(this IServiceProvider serviceProvider)
	{
		using var scope = serviceProvider.CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

		if (dbContext.Database.GetService<IRelationalDatabaseCreator>() is RelationalDatabaseCreator databaseCreator)
		{
			if (!databaseCreator.Exists())
			{
				dbContext.Database.Migrate();
			}
			else
			{
				var pendingMigrations = dbContext.Database.GetPendingMigrations();
				if (pendingMigrations.Any())
				{
					dbContext.Database.Migrate();
				}
			}
		}
	}
}