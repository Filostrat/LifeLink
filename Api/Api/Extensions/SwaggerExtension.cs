using Microsoft.OpenApi.Models;


namespace Api.Extensions;

public static class SwaggerExtension
{
	public static void AddSwaggerDoc(this IServiceCollection services)
	{
		services.AddSwaggerGen(c =>
		{
			c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			{
				Description = @"JWT Authorization header using the Bearer scheme. 
                                  Enter your token in the text input below.
                                  Example: 12345abcdef",
				Name = "Authorization",
				In = ParameterLocation.Header,
				Type = SecuritySchemeType.Http,
				Scheme = "bearer",
				BearerFormat = "JWT"
			});

			c.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						},
						Scheme = "bearer",
						Name = "Bearer",
						In = ParameterLocation.Header,
					},
					new List<string>()
				}
			});

			c.SwaggerDoc("v1", new OpenApiInfo
			{
				Version = "v1",
				Title = "Life Link Api",
			});
		});
	}
}
