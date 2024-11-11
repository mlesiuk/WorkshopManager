using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;
using workshopManager.Api.Endpoints;

namespace workshopManager.Api.Extensions;

public static class EnpointExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        ServiceDescriptor[] s = assembly.DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(s);
        return services;
    }

    public static IApplicationBuilder MapEndpoints(this WebApplication app)
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();
        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(app);
        }
        return app;
    }
}
