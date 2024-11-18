using Mapster;
using MediatR;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Application.Dtos;

namespace workshopManager.Application.Queries.VehicleFuelType;

public sealed record class GetAllVehicleFuelTypesQuery : IRequest<List<VehicleFuelTypeDto>>;

public sealed class GetAllVehicleFuelTypesQueryHandler : IRequestHandler<GetAllVehicleFuelTypesQuery, List<VehicleFuelTypeDto>>
{
    private readonly IVehicleFuelTypeRepository _vehicleFuelTypeRepository;

    public GetAllVehicleFuelTypesQueryHandler(IVehicleFuelTypeRepository vehicleFuelTypeRepository)
    {
        _vehicleFuelTypeRepository = vehicleFuelTypeRepository;
    }

    public async Task<List<VehicleFuelTypeDto>> Handle(GetAllVehicleFuelTypesQuery request, CancellationToken cancellationToken)
    {
        var entities = await _vehicleFuelTypeRepository.GetAllAsync(cancellationToken);
        return entities.Adapt<List<VehicleFuelTypeDto>>();
    }
}
