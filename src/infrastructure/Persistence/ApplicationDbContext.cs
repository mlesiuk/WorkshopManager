using Microsoft.EntityFrameworkCore;
using System.Reflection;
using workshopManager.Domain.Entities;

namespace workshopManager.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        ConfigureTables<Customer>(builder);
        ConfigureTables<ServiceAssignment>(builder);
        ConfigureTables<VehicleAdditionalEquipment>(builder);
        ConfigureTables<VehicleBodyType>(builder);
        ConfigureTables<VehicleBrand>(builder);
        ConfigureTables<VehicleEngine>(builder);
        ConfigureTables<VehicleFuelType>(builder);
        ConfigureTables<VehicleGearbox>(builder);
        ConfigureTables<VehicleGeneration>(builder);
        ConfigureTables<VehicleModel>(builder);
        ConfigureTables<VehiclePropulsion>(builder);
        #region Vehicle
        builder.Entity<Vehicle>()
            .Property<DateTime>("ManufactureDate")
            .IsRequired()
            .HasColumnType("date");

        builder.Entity<Vehicle>()
            .Property<string>("RegistrationNumber")
            .IsRequired()
            .HasMaxLength(16)
            .HasColumnType("varchar");

        builder.Entity<Vehicle>()
            .Property<string>("VinNumber")
            .IsRequired()
            .HasMaxLength(32)
            .HasColumnType("varchar");

        builder.Entity<Vehicle>()
            .HasMany(v => v.AdditionalEquipment)
            .WithOne();

        builder.Entity<Vehicle>()
            .Navigation(b => b.AdditionalEquipment)
            .UsePropertyAccessMode(PropertyAccessMode.Property);
        #endregion

        base.OnModelCreating(builder);
    }

    protected static void ConfigureTables<T>(ModelBuilder modelBuilder) where T : BaseEntity
    {
        modelBuilder.Entity<T>()
            .ToTable(typeof(T).Name)
            .HasKey(e => e.Id)
            .IsClustered(false);

        modelBuilder.Entity<T>()
            .HasIndex(e => e.ClusterId)
            .IsClustered(true)
            .IsUnique(true);

        modelBuilder.Entity<T>()
            .Property<string>("Name")
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnType("varchar");

        modelBuilder.Entity<T>()
            .Property(p => p.Id)
            .HasDefaultValueSql("newId()")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<T>()
            .Property(p => p.Created)
            .HasDefaultValueSql("getUtcDate()")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<T>()
            .Property(p => p.ClusterId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<T>()
            .HasAlternateKey(p => p.ClusterId);
    }
}
