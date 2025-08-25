using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UtisTestTask.Core.Base.Requests;
using UtisTestTask.Core.Services;
using UtisTestTask.Currency;
using UtisTestTask.DAL;
using UtisTestTask.Rabbit;

namespace UtisTestTask.Core;

public static class CoreDependencyInjection
{
    /// <summary>
    /// Register core services and add core configurations
    /// </summary>
    /// <param name="services">Services</param>
    /// <param name="configuration">Configuration</param>
    /// <returns></returns>
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureDatabase(configuration);
        services.AddRabbit(configuration);
        services.AddCurrency(configuration);
        services.AddValidatorsFromAssemblyContaining(typeof(CoreDependencyInjection));
        RegisterCommands(services);
        RegisterServices(services);

        return services;
    }

    private static void RegisterServices(IServiceCollection services)
    {
        // Transient
        services.AddTransient<IRequestHandler, RequestHandler>();

        // Hosted
        services.AddHostedService<OverdueTasksHostedService>();
    }

    private static void RegisterCommands(IServiceCollection services)
    {
        var implementations = GetImplementationsOfGenericClass(typeof(BaseRequest<,>));
        foreach (var type in implementations)
        {
            services.AddTransient(type);
        }
    }

    private static IEnumerable<Type> GetImplementationsOfGenericClass(Type baseClass) => typeof(CoreDependencyInjection).Assembly
        .GetTypes()
        .Where(type => IsSubclassOfGenericType(type, baseClass))
        .ToList();

    private static bool IsSubclassOfGenericType(Type type, Type genericTypeDefinition)
    {
        if (!genericTypeDefinition.IsGenericTypeDefinition) return false;
        if (type.IsAbstract) return false;

        var currentType = type.BaseType;
        while (currentType != null && currentType != typeof(object))
        {
            if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == genericTypeDefinition)
            {
                return true;
            }
            currentType = currentType.BaseType;
        }
        return false;
    }
}