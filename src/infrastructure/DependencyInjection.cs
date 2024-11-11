using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Infrastructure.Persistence;
using workshopManager.Infrastructure.Repositories;
using workshopManager.Infrastructure.Services;

namespace workshopManager.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IVehicleBrandRepository, VehicleBrandRepository>();

        var secretClientName = configuration.GetSection("SecretClient:Name")?.Value;
        services.TryAddKeyedSingleton(secretClientName, (sp, obj) =>
        {
            if (!string.IsNullOrEmpty(secretClientName))
            {
                var uri = string.Format("https://{0}.vault.azure.net", secretClientName);
                return new SecretClient(new Uri(uri), new DefaultAzureCredential());
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

            options.UseSqlServer(configuration.GetConnectionString(connectionString),
                b => b.MigrationsAssembly(assembly.FullName));
        });

        return services;
    }
}
