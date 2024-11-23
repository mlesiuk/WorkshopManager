namespace workshopManager.Domain.Entities;

public sealed class ServiceAssignment : BaseEntity
{
    public Customer? Customer { get; set; }
    public Vehicle? Vehicle { get; set; }

    private ServiceAssignment(Customer customer, Vehicle vehicle)
    {
        Customer = customer;
        Vehicle = vehicle;
    }

    public static ServiceAssignment Create(Customer customer, Vehicle vehicle)
    {
        return new ServiceAssignment(customer, vehicle);
    }
}
