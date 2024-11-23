using Mapster;
using MediatR;
using OneOf;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;

namespace workshopManager.Application.Queries.Customer;

public sealed record class GetCustomerQuery : CustomerDto, IRequest<OneOf<CustomerDto, NotFoundException>>
{
    public GetCustomerQuery(Guid id)
    {
        Id = id;
    }
}

public sealed class GetCustomerQueryHandler
    : IRequestHandler<GetCustomerQuery, OneOf<CustomerDto, NotFoundException>>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<OneOf<CustomerDto, NotFoundException>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        var entity = await _customerRepository.GetByIdAsync(request.Id, cancellationToken) ??
            await _customerRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (entity == null)
        {
            return new NotFoundException($"Customer type with ID {request.Id} not found.");
        }

        return entity.Adapt<CustomerDto>();
    }
}
