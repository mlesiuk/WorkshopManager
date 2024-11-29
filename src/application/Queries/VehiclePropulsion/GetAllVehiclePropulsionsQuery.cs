using Mapster;
using MediatR;
using workshopManager.Application.Dtos;
using workshopManager.Domain.Abstractions.Interfaces;

namespace workshopManager.Application.Queries.VehiclePropulsion;

public sealed record class GetAllVehiclePropulsionsQuery : IRequest<List<VehiclePropulsionDto>>;

public sealed class GetAllVehiclePropulsionsQueryHandler : IRequestHandler<GetAllVehiclePropulsionsQuery, List<VehiclePropulsionDto>>
{
    private readonly IVehiclePropulsionRepository _vehiclePropulsionRepository;

    public GetAllVehiclePropulsionsQueryHandler(IVehiclePropulsionRepository vehiclePropulsionRepository)
    {
        _vehiclePropulsionRepository = vehiclePropulsionRepository;
    }

    public async Task<List<VehiclePropulsionDto>> Handle(GetAllVehiclePropulsionsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _vehiclePropulsionRepository.GetAllAsync(cancellationToken);
        return entities.Adapt<List<VehiclePropulsionDto>>();
    }
}
