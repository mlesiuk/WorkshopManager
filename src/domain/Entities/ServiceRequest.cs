using workshopManager.Domain.Enums;

namespace workshopManager.Domain.Entities;

public sealed class ServiceRequest : BaseEntity
{
    public Customer? Customer { get; set; }
    public Vehicle? Vehicle { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ExpectedEndDate { get; set; }
    public ServiceStatus Status { get; }
    public IEnumerable<Service> Services { get; set; } = [];
    public string Summary { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;

    private ServiceRequest(Customer customer, Vehicle vehicle, DateTime startDate, ServiceStatus status)
    {
        Customer = customer;
        Vehicle = vehicle;
        StartDate = startDate;
        Status = status;
    }

    public static ServiceRequest Create(Customer customer, Vehicle vehicle, DateTime startDate)
    {
        return new ServiceRequest(customer, vehicle, startDate, ServiceStatus.New);
    }
}
