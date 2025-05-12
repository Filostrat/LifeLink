using EmailBackgroundService;
using Application.Extensions;
using Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<EmailConsumerHostedService>();

builder.Services.ConfigureApplicationServices();
builder.Services.ConfigureInfrastructureServices(builder.Configuration);

var host = builder.Build();

host.Run();