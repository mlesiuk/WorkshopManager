using MediatR;
using workshopManager.Application.Abstractions.Interfaces;

namespace workshopManager.Application.Queries;

public class GetElementByIdQuery<T> : IRequest<T> where T : class
{
    public Guid Id { get; set; }

    public GetElementByIdQuery(Guid id)
    {
        Id = id;
    }
}

public class GetElementByIdHandler<T> : IRequestHandler<GetElementByIdQuery<T>, T?> where T : class
{
    private readonly IApplicationDbContextAsync<T> _context;

    public GetElementByIdHandler(IApplicationDbContextAsync<T> context)
    {
        _context = context;
    }

    public async Task<T?> Handle(GetElementByIdQuery<T> request, CancellationToken cancellationToken = default)
    {
        var element = await _context.Entities.FindAsync(request.Id, cancellationToken);
        if (element is not null)
        {
            return element;
        }
        return default;
    }
}
