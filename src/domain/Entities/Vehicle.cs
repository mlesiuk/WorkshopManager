namespace workshopManager.Domain.Entities;

public class Vehicle : BaseEntity
{
    public List<VehicleAdditionalEquipment>? AdditionalEquipment { get; set; }
    public VehicleBodyType BodyType { get; set; } = new();
    public VehicleBrand? Brand { get; set; }
    public VehicleEngine Engine { get; set; } = new();
    public VehicleFuelType FuelType { get; set; } = new();
    public VehicleGearbox Gearbox { get; set; } = new();
    public VehicleGeneration Generation { get; set; } = new();
    public DateTime ManufactureDate { get; set; }
    public VehicleModel Model { get; set; } = new();
    public VehiclePropulsion Propulsion { get; set; } = new();
    public string? RegistrationNumber { get; set; }
    public string? VinNumber { get; set; }
}
