using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;

using System.IdentityModel.Tokens.Jwt;
using System.Reflection;

using UI.Contracts;
using UI.Extensions;
using UI.Services;
using UI.Services.Base;
using UI.Validators;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
	options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddFluentValidationAutoValidation(options =>
{
	options.DisableDataAnnotationsValidation = true;
});

builder.Services.AddValidatorsFromAssemblyContaining<RegisterVMValidator>();

builder.Services.AddSingleton<JwtSecurityTokenHandler>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();

builder.Services.AddTransient<IBloodTypeService, BloodTypeService>();

builder.Services.AddTransient<IDonorService, DonorService>();

builder.Services.AddSingleton<ILocalStorageService, LocalStorageService>();

builder.Services.AddTransient<ApiClientLoggingHandler>();

builder.Services
	.AddHttpClient("ApiClient");
	//.AddHttpMessageHandler<ApiClientLoggingHandler>();


builder.Services.AddTransient<IClient>(serviceProvider =>
{
	var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
	var httpClient = httpClientFactory.CreateClient("ApiClient");

	var baseUrl = Environment.GetEnvironmentVariable("BASE_API_URL");

	return new Client(baseUrl, httpClient);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseCookiePolicy();

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}")
	.WithStaticAssets();

app.Run();