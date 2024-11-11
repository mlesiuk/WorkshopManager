namespace workshopManager.Application.Exceptions;

public sealed class NotFoundException : Exception
{
    public NotFoundException() : base($"Entity not found.") { }

    public NotFoundException(string entityName) : base($"Entity {entityName} not found.") { }
}
