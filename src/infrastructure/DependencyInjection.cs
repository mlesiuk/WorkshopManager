using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Infrastructure.Persistence;
using workshopManager.Infrastructure.Repositories;

namespace workshopManager.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = typeof(ApplicationDbContext).Assembly;
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("WorkshopManagerConnection"),
            b => b.MigrationsAssembly(assembly.FullName)));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IVehicleBrandRepository, VehicleBrandRepository>();

        return services;
    }
}
