using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace workshopManager.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddValidatorsFromAssembly(assembly);
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));

        return services;
    }
}
