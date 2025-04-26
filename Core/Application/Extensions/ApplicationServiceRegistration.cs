using Application.DTOs.Account.Requests;
using Application.DTOs.Account.Validators;

using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.Extensions.DependencyInjection;

using System.Reflection;


namespace Application.Extensions;

public static class ApplicationServiceRegistration
{
	public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
	{
		services.AddAutoMapper(Assembly.GetExecutingAssembly());
		services.AddMediatR(cfg =>
			cfg.RegisterServicesFromAssembly(typeof(ApplicationServiceRegistration).Assembly));

		services.AddFluentValidationAutoValidation(options =>
		{
			options.DisableDataAnnotationsValidation = true;
		});

		services.AddValidatorsFromAssembly(typeof(ApplicationServiceRegistration).Assembly,includeInternalTypes: true);

		return services;
	}
}