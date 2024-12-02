using Azure.Identity;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using workshopManager.Domain.Abstractions.Interfaces;
using workshopManager.Infrastructure.Models;
using workshopManager.Infrastructure.Persistence;
using workshopManager.Infrastructure.Repositories;
using workshopManager.Infrastructure.Services;
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

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IServiceRequestRepository, ServiceRequestRepository>();
        services.AddScoped<IVehicleAdditionalEquipmentRepository, VehicleAdditionalEquipmentRepository>();
        services.AddScoped<IVehicleBodyTypeRepository, VehicleBodyTypeRepository>();
        services.AddScoped<IVehicleBrandRepository, VehicleBrandRepository>();
        services.AddScoped<IVehicleEngineRepository, VehicleEngineRepository>();
        services.AddScoped<IVehicleFuelTypeRepository, VehicleFuelTypeRepository>();
        services.AddScoped<IVehicleGearboxRepository, VehicleGearboxRepository>();
        services.AddScoped<IVehicleGenerationRepository, VehicleGenerationRepository>();
        services.AddScoped<IVehicleModelRepository, VehicleModelRepository>();
        services.AddScoped<IVehiclePropulsionRepository, VehiclePropulsionRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IWorkerRepository, WorkerRepository>();

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
