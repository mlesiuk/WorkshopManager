namespace workshopManager.Application.Exceptions;

public sealed class CustomerNotFoundException : Exception
{
    public CustomerNotFoundException() : base($"Customer not found.") { }

    public CustomerNotFoundException(Guid customerId) : base($"Customer with id {customerId} not found.") { }
}
