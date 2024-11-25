using workshopManager.Domain.Enums;
using workshopManager.Domain.ValueObjects;

namespace workshopManager.Domain.Entities;

public sealed class Worker : BaseEntity
{
    public Name Name { get; }

    private readonly List<ServiceRequest> _serviceRequests = [];
    public IEnumerable<ServiceRequest> ServiceRequests => _serviceRequests;

    private WorkersPosition position;
    public WorkersPosition Position => position;

    private Worker(string firstName, string lastName)
    {
        Name = new Name(firstName, lastName);
    }

    public static Worker Create(string firstName, string lastName)
    {
        return new Worker(firstName, lastName);
    }

    public void SetWorkersPosition(WorkersPosition workersPosition)
    {
        position = workersPosition;
    }

    public void AssignServiceRequest(ServiceRequest serviceRequest)
    {
        if (_serviceRequests.Any(sr => sr.Id == serviceRequest.Id)) 
        {
            throw new InvalidOperationException($"ServiceRequest with id {serviceRequest.Id} is already assigned to worker {Name}.");
        }
        _serviceRequests.Add(serviceRequest);
    }

    public bool IsAvailableOnSelectedDate(DateTime date)
    {
        return _serviceRequests.Any(sr => sr.StartDate < date);
    }
}
