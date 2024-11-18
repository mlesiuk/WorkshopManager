namespace workshopManager.Domain.Entities;

public class Vehicle : BaseEntity
{
    public List<VehicleAdditionalEquipment>? AdditionalEquipment { get; set; }
    public VehicleBodyType? BodyType { get; set; }
    public VehicleBrand? Brand { get; set; }
    public VehicleEngine? Engine { get; set; }
    public VehicleFuelType? FuelType { get; set; }
    public VehicleGearbox? Gearbox { get; set; }
    public VehicleGeneration Generation { get; set; } = new();
    public DateTime ManufactureDate { get; set; }
    public VehicleModel Model { get; set; } = new();
    public VehiclePropulsion Propulsion { get; set; } = new();
    public string? RegistrationNumber { get; set; }
    public string? VinNumber { get; set; }
}
