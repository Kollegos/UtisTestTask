using UtisTestTask.Consumer;
using UtisTestTask.Consumer.Base;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var settingsSection = builder.Configuration.GetRequiredSection(RabbitSettings.ConfigurationSection);
builder.Services.Configure<RabbitSettings>(settingsSection);
var settings = settingsSection.Get<RabbitSettings>();
if (string.IsNullOrEmpty(settings?.Host) || string.IsNullOrEmpty(settings?.TaskOverdueQueue)) throw new ArgumentNullException(nameof(RabbitSettings), "Rabbit not configured in AppSetting. \"RabbitSettings\" section expected.");

builder.Services.AddSingleton(settings);
builder.Services.AddHostedService<TaskOverdueConsumerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.Run();
