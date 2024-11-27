using queue_api;
using queue_services.QueueService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddLogging(i =>
{
	i.AddDebug();
	i.AddConsole();
});

builder.Services.AddControllers();

builder.Services.AddSingleton<IQueueService<string>, QueueService>();

builder.Services.AddHostedService<QueueHostedService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();