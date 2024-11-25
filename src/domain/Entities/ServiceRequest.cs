using workshopManager.Domain.Enums;

namespace workshopManager.Domain.Entities;

public sealed class ServiceRequest : BaseEntity
{
    public Customer? Customer { get; set; }
    public Vehicle? Vehicle { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ExpectedEndDate { get; set; }
    public ServiceStatus Status { get; set; }
    public IEnumerable<Service> Services { get; set; } = [];
    public string Summary { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;

    private ServiceRequest(Customer customer, Vehicle vehicle, ServiceStatus status)
    {
        Customer = customer;
        Vehicle = vehicle;
    }

    public static ServiceRequest Create(Customer customer, Vehicle vehicle, ServiceStatus status)
    {
        return new ServiceRequest(customer, vehicle, status);
    }
}
