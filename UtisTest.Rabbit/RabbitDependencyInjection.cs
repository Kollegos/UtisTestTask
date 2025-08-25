using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UtisTestTask.Rabbit.Base;
using UtisTestTask.Rabbit.Producers;

namespace UtisTestTask.Rabbit;

public static class RabbitDependencyInjection
{
    public static IServiceCollection AddRabbit(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ITaskOverdueProducer, TaskOverdueProducer>();

        // Add Configuration
        var settingsSection = configuration.GetRequiredSection(RabbitSettings.ConfigurationSection);
        services.Configure<RabbitSettings>(settingsSection);
        var settings = settingsSection.Get<RabbitSettings>();
        if(string.IsNullOrEmpty(settings?.Host) || string.IsNullOrEmpty(settings?.TaskOverdueQueue)) throw new ArgumentNullException(nameof(RabbitSettings), "Rabbit not configured in AppSetting. \"RabbitSettings\" section expected.");
        
        services.AddSingleton(settings);

        return services;
    }
    
}