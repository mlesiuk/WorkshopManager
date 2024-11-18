using Mapster;
using MediatR;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Application.Dtos;

namespace workshopManager.Application.Queries.VehicleBodyType;

public sealed record class GetAllVehicleBodyTypesQuery : IRequest<List<VehicleBodyTypeDto>>;

public sealed class GetAllVehicleBodyTypesQueryHandler : IRequestHandler<GetAllVehicleBodyTypesQuery, List<VehicleBodyTypeDto>>
{
    private readonly IVehicleBodyTypeRepository _vehileBodyTypeRepository;

    public GetAllVehicleBodyTypesQueryHandler(IVehicleBodyTypeRepository vehicleBodyTypeRepository)
    {
        _vehileBodyTypeRepository = vehicleBodyTypeRepository;
    }

    public async Task<List<VehicleBodyTypeDto>> Handle(GetAllVehicleBodyTypesQuery request, CancellationToken cancellationToken)
    {
        var entities = await _vehileBodyTypeRepository.GetAllAsync(cancellationToken);
        return entities.Adapt<List<VehicleBodyTypeDto>>();
    }
}
