using Mapster;
using MediatR;
using workshopManager.Application.Dtos;
using workshopManager.Domain.Abstractions.Interfaces;

namespace workshopManager.Application.Queries.VehicleEngine;

public sealed record class GetAllVehicleEnginesQuery : IRequest<List<VehicleEngineDto>>;

public sealed class GetAllVehicleEnginesQueryHandler : IRequestHandler<GetAllVehicleEnginesQuery, List<VehicleEngineDto>>
{
    private readonly IVehicleEngineRepository _vehicleEngineRepository;

    public GetAllVehicleEnginesQueryHandler(IVehicleEngineRepository vehicleEngineRepository)
    {
        _vehicleEngineRepository = vehicleEngineRepository;
    }

    public async Task<List<VehicleEngineDto>> Handle(GetAllVehicleEnginesQuery request, CancellationToken cancellationToken)
    {
        var entities = await _vehicleEngineRepository.GetAllAsync(cancellationToken);
        return entities.Adapt<List<VehicleEngineDto>>();
    }
}