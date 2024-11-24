namespace workshopManager.Application.Exceptions;

public sealed class NotFoundException : Exception
{
    public NotFoundException() : base($"Entity not found.") { }

    public NotFoundException(string entityName) : base($"Entity {entityName} not found.") { }

    public NotFoundException(string entityName, Guid entityId) : base($"Entity {entityName} with id {entityId} not found.") { }
}
