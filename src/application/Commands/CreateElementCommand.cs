using MediatR;
using workshopManager.Application.Abstractions.Interfaces;

namespace workshopManager.Application.Commands;

public class CreateElementCommand<T> : IRequest<Guid> where T : class
{
    public T Element { get; set; }

    public CreateElementCommand(T element)
    {
        Element = element;
    }
}

public class CreateElementCommandHandler<T> : IRequestHandler<CreateElementCommand<T>, Guid> where T : class
{
    private readonly IApplicationDbContextAsync<T> _context;
    private readonly IEntityOperations<T> _entityOperations;

    public CreateElementCommandHandler(
        IApplicationDbContextAsync<T> context,
        IEntityOperations<T> entityOperations)
    {
        _context = context;
        _entityOperations = entityOperations;
    }

    public async Task<Guid> Handle(CreateElementCommand<T> request, CancellationToken cancellationToken = default)
    {
        //var validationResult = _validator.Validate(request.Element);

        //var failures = validationResult.Errors?.ToList();

        //if (failures?.Count > 0)
        //    throw new ValidationException(failures);

        var element = _entityOperations.CreateElement(request.Element);

        if (_entityOperations.AlreadyExists(element, _context.Entities))
        {
            throw new Exception($"Entity {_entityOperations.GetElementName(element)} already exists");
        }

        _context.Entities.Add(element);

        await _context.SaveChangesAsync(cancellationToken);

        return _entityOperations.GetElementId(element);
    }
}
