using Mapster;
using MediatR;
using workshopManager.Application.Dtos;
using workshopManager.Domain.Abstractions.Interfaces;

namespace workshopManager.Application.Queries.VehicleModel;

public sealed record class GetAllVehicleModelsQuery : IRequest<List<VehicleModelDto>>;

public sealed class GetAllVehicleModelsQueryHandler : IRequestHandler<GetAllVehicleModelsQuery, List<VehicleModelDto>>
{
    private readonly IVehicleModelRepository _vehicleModelRepository;

    public GetAllVehicleModelsQueryHandler(IVehicleModelRepository vehicleModelRepository)
    {
        _vehicleModelRepository = vehicleModelRepository;
    }

    public async Task<List<VehicleModelDto>> Handle(GetAllVehicleModelsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _vehicleModelRepository.GetAllAsync(cancellationToken);
        return entities.Adapt<List<VehicleModelDto>>();
    }
}
