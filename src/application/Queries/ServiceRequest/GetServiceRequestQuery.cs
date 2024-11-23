using Mapster;
using MediatR;
using OneOf;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;

namespace workshopManager.Application.Queries.ServiceRequest;

public sealed record class GetServiceRequestQuery : ServiceRequestDto, IRequest<OneOf<ServiceRequestDto, NotFoundException>>;

public sealed class GetServiceRequestQueryHandler
    : IRequestHandler<GetServiceRequestQuery, OneOf<ServiceRequestDto, NotFoundException>>
{
    private readonly IVehicleBodyTypeRepository _vehicleBodyTypeRepository;

    public GetServiceRequestQueryHandler(IVehicleBodyTypeRepository vehicleBodyTypeRepository)
    {
        _vehicleBodyTypeRepository = vehicleBodyTypeRepository;
    }

    public async Task<OneOf<ServiceRequestDto, NotFoundException>> Handle(GetServiceRequestQuery request, CancellationToken cancellationToken)
    {
        var entity = await _vehicleBodyTypeRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
        {
            return new NotFoundException($"Body type with ID {request.Id} not found.");
        }

        return entity.Adapt<ServiceRequestDto>();
    }
}
