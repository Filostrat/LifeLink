using Api.Extensions;
using Api.Services;
using Api.Services.Contracts;

using Application.Extensions;

using Identity;

using Infrastructure;
using Persistence;
using Serilog;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerDoc();

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddScoped<IHttpContextService, HttpContextService>();

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddHttpContextAccessor();

builder.Services.AddOpenApi();


builder.Services.ConfigureApplicationServices();
builder.Services.ConfigureIdentityServices(builder.Configuration);
builder.Services.ConfigureInfrastructureServices(builder.Configuration);
builder.Services.ConfigurePersistenceServices(builder.Configuration);


builder.Services.AddSwaggerGenNewtonsoftSupport();

var app = builder.Build();

app.Services.ApplyIdentityMigrations();
app.Services.ApplyPersistenceMigrations();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "lifelink.Api v1");
	});
}

app.UseExceptionMiddleware();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();