using workshopManager.Domain.Enums;
using workshopManager.Domain.ValueObjects;

namespace workshopManager.Domain.Entities;

public sealed class Customer : BaseEntity
{
    public Name Name { get; set; }
    public IDictionary<AddressType, Address> Addresses { get; set; } = new Dictionary<AddressType, Address>();
    public string Phone { get; set; }
    public string Email { get; set; }
    public ICollection<Vehicle> Vehicles { get; set; } = [];

    private Customer(
        string firstName,
        string lastName,
        string phone,
        string email)
    {
        Name = new Name(firstName, lastName);
        Email = email;
        Phone = phone;
    }

    public static Customer Create(
        string firstName,
        string lastName,
        string phone,
        string email)
    {
        return new Customer(firstName, lastName, phone, email);
    }

    public void AddHomeAddress(
        string city,
        string street,
        string number,
        string apartment,
        string postalCode,
        string country,
        string region)
    {
        AddAddress(AddressType.Home, city, street, number, apartment, postalCode, country, region);
    }

    public void AddWorkAddress(
        string city,
        string street,
        string number,
        string apartment,
        string postalCode,
        string country,
        string region)
    {
        AddAddress(AddressType.Work, city, street, number, apartment, postalCode, country, region);
    }

    public void AddDeliveryAddress(
        string city,
        string street,
        string number,
        string apartment,
        string postalCode,
        string country,
        string region)
    {
        AddAddress(AddressType.Delivery, city, street, number, apartment, postalCode, country, region);
    }

    public void AddVehicle(Vehicle vehicle)
    {
        if (Vehicles.Contains(vehicle))
        {
            throw new InvalidOperationException("Vehicle is already binded to Customer");
        }

        Vehicles.Add(vehicle);
    }

    public void RemoveVehicle(Vehicle vehicle)
    {
        if (!Vehicles.Contains(vehicle))
        {
            throw new InvalidOperationException("Vehicle is not binded to Customer");
        }

        Vehicles.Remove(vehicle);
    }

    private void AddAddress(
        AddressType type,
        string city,
        string street,
        string number,
        string apartment,
        string postalCode,
        string country,
        string region)
    {
        var address = new Address(city, street, number, apartment, postalCode, country, region);
        if (!Addresses.ContainsKey(type))
        {
            Addresses.Add(type, address);
        }
        else
        {
            Addresses[type] = address;
        }
    }
}
