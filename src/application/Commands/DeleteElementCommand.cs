using MediatR;
using workshopManager.Application.Abstractions.Interfaces;

namespace workshopManager.Application.Commands;

public class DeleteElementCommand<T> : IRequest<Unit> where T : class
{
    public Guid Id { get; set; }
    public DeleteElementCommand(Guid id)
    {
        Id = id;
    }
}

public class DeleteElementCommandHandler<T> : IRequestHandler<DeleteElementCommand<T>, Unit> where T : class
{
    private readonly IApplicationDbContextAsync<T> _context;

    public DeleteElementCommandHandler(IApplicationDbContextAsync<T> context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteElementCommand<T> request, CancellationToken cancellationToken)
    {
        var element = await _context.Entities.FindAsync(new object[] { request.Id }, cancellationToken);

        if (element == null)
        {
            throw new ArgumentNullException(nameof(element));
        }

        _context.Entities.Remove(element);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
