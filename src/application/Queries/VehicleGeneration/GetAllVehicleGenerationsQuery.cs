using Mapster;
using MediatR;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Application.Dtos;

namespace workshopManager.Application.Queries.VehicleGeneration;

public sealed record class GetAllVehicleGenerationsQuery : IRequest<List<VehicleGenerationDto>>;

public sealed class GetAllVehicleGenerationsQueryHandler : IRequestHandler<GetAllVehicleGenerationsQuery, List<VehicleGenerationDto>>
{
    private readonly IVehicleGenerationRepository _vehicleGenerationRepository;

    public GetAllVehicleGenerationsQueryHandler(IVehicleGenerationRepository vehicleGenerationRepository)
    {
        _vehicleGenerationRepository = vehicleGenerationRepository;
    }

    public async Task<List<VehicleGenerationDto>> Handle(GetAllVehicleGenerationsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _vehicleGenerationRepository.GetAllAsync(cancellationToken);
        return entities.Adapt<List<VehicleGenerationDto>>();
    }
}
