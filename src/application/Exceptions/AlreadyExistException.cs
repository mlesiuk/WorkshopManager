namespace workshopManager.Application.Exceptions;

public sealed class AlreadyExistException(string entityName) : Exception($"Entity {entityName} already exists.") { }
