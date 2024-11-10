using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace workshopManager.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(configuration => Assembly.GetExecutingAssembly());

        return services;
    }
}
