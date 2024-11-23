using workshopManager.Domain.Common;

namespace workshopManager.Domain.ValueObjects;

public sealed class Address : ValueObject
{
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Apartment { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;

    public Address(
        string city, 
        string street, 
        string number, 
        string apartment, 
        string postalCode, 
        string country, 
        string region)
    {
        City = city;
        Street = street;
        Number = number;
        Apartment = apartment;
        Region = region;
        PostalCode = postalCode;
        Country = country;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return City;
        yield return Country;
        yield return PostalCode;
        yield return Number;
        yield return Apartment;
        yield return Region;
        yield return Street;
    }
}
