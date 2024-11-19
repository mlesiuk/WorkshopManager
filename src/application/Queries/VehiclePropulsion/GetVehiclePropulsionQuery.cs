using Mapster;
using MediatR;
using OneOf;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;

namespace workshopManager.Application.Queries.VehiclePropulsion;

public sealed record class GetVehiclePropulsionQuery(Guid Id) : IRequest<OneOf<VehiclePropulsionDto, NotFoundException>>;

public sealed class GetVehiclePropulsionByIdQueryHandler : IRequestHandler<GetVehiclePropulsionQuery, OneOf<VehiclePropulsionDto, NotFoundException>>
{
    private readonly IVehiclePropulsionRepository _vehiclePropulsionRepository;

    public GetVehiclePropulsionByIdQueryHandler(IVehiclePropulsionRepository vehiclePropulsionRepository)
    {
        _vehiclePropulsionRepository = vehiclePropulsionRepository;
    }

    public async Task<OneOf<VehiclePropulsionDto, NotFoundException>> Handle(GetVehiclePropulsionQuery request, CancellationToken cancellationToken)
    {
        var entity = await _vehiclePropulsionRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
        {
            return new NotFoundException($"VehiclePropulsion with ID {request.Id} not found.");
        }

        return entity.Adapt<VehiclePropulsionDto>();
    }
}
