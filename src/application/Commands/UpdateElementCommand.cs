using MediatR;
using workshopManager.Application.Abstractions.Interfaces;

namespace workshopManager.Application.Commands;

public class UpdateElementCommand<T> : IRequest<Unit> where T : class
{
    public Guid Id { get; set; }
    public T Element { get; set; }
    
    public UpdateElementCommand(T element)
    {
        Element = element;
    }

    public UpdateElementCommand(Guid id, T element)
    {
        Id = id;
        Element = element;
    }
}

public class UpdateElementCommandHandler<T> : IRequestHandler<UpdateElementCommand<T>, Unit> where T : class
{
    private readonly IApplicationDbContextAsync<T> _context;
    private readonly IEntityOperations<T> _entityOperations;

    public UpdateElementCommandHandler(IApplicationDbContextAsync<T> context, IEntityOperations<T> elementOperations)
    {
        _context = context;
        _entityOperations = elementOperations;
    }

    public async Task<Unit> Handle(UpdateElementCommand<T> request, CancellationToken cancellationToken)
    {
        var element = await _context.Entities.FindAsync(new object[] { request.Id }, cancellationToken);

        if (element == null)
        {
            throw new ArgumentNullException(nameof(request.Element));
        }

        _entityOperations.UpdateElement(element, request.Element);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
