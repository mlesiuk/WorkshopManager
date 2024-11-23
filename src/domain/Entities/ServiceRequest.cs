using workshopManager.Domain.Enums;

namespace workshopManager.Domain.Entities;

public sealed class ServiceRequest : BaseEntity
{
    public Customer? Customer { get; set; }
    public Vehicle? Vehicle { get; set; }
    public ServiceStatus Status { get; set; }

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
