using Azure.Identity;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using workshopManager.Domain;
using workshopManager.Domain.Abstractions.Interfaces;
using workshopManager.Infrastructure.Models;
using workshopManager.Infrastructure.Persistence;
using workshopManager.Infrastructure.Repositories;
using workshopManager.Infrastructure.Services;
using DomainAssembly = workshopManager.Domain.AssemblyReference;
using InfrastructureAssembly = workshopManager.Infrastructure.AssemblyReference;
using SecretClient = Azure.Security.KeyVault.Secrets.SecretClient;

namespace workshopManager.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configurationSection)
    {
        services.AddRepositories();

        var configuration = new Configuration();
        configurationSection.GetSection("Configuration")
            .Bind(configuration);

        services.AddMassTransitWithRabbitMq(configuration.RabbitMq);

        var secretClientName = configuration.SecretClient.Name;
        services.TryAddKeyedSingleton(secretClientName, (serviceProvider, obj) =>
        {
            if (!string.IsNullOrEmpty(secretClientName))
            {
                var uri = string.Format("https://{0}.vault.azure.net", secretClientName);
                return new SecretClient(new Uri(uri), new AzureCliCredential());
            }
            else
            {
                throw new ArgumentNullException(nameof(secretClientName));
            }
        });

        services.TryAddKeyedSingleton(secretClientName, (serviceProvider, obj) =>
        {
            var secretClient = serviceProvider.GetKeyedService<SecretClient>(secretClientName);
            return secretClient == null ? throw new Exception() : new SecretsService(secretClient);
        });

        var assembly = typeof(ApplicationDbContext).Assembly;
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            var secretClient = sp.GetRequiredKeyedService<SecretsService>(secretClientName);
            var connectionString = secretClient
                .GetSecretAsync("WorkshopManagerConnection")
                .GetAwaiter()
                .GetResult();

            options.UseSqlServer(connectionString,
                b => b.MigrationsAssembly(assembly.FullName));
        });

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        var interfaces = typeof(DomainAssembly).Assembly.DefinedTypes
            .Where(type => type is { IsAbstract: true, IsInterface: true } &&
                type.Name.EndsWith("Repository"))
            .ToList();

        var implementations = typeof(InfrastructureAssembly).Assembly.DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                type.Name.EndsWith("Repository"))
            .ToList();

        foreach(var @interface in interfaces)
        {
            var implementation = implementations.FirstOrDefault(i => i.IsAssignableTo(@interface));
            if (implementation is not null)
            {
                services.AddScoped(@interface, implementation);
            }
        }

        return services;
    }

    private static IServiceCollection AddMassTransitWithRabbitMq(this IServiceCollection services,
        RabbitMqConfiguration config)
    {
        services.AddMassTransit(configuration =>
        {
            configuration.SetKebabCaseEndpointNameFormatter();
            configuration.UsingRabbitMq((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
                cfg.Host(new Uri(config.Uri), rmhc =>
                {
                    rmhc.Username(config.Username);
                    rmhc.Password(config.Password);
                });
            });
        });

        return services;
    }
}
