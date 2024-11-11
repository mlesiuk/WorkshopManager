using Mapster;
using MediatR;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Application.Dtos;

namespace workshopManager.Application.Queries.VehicleBrand;

public sealed record class GetVehicleBrandsQuery : VehicleBrandDto, IRequest<IEnumerable<VehicleBrandDto>> { }

public sealed class GetVehicleBrandsQueryHandler(IVehicleBrandRepository repository) : IRequestHandler<GetVehicleBrandsQuery, IEnumerable<VehicleBrandDto>>
{
    public async Task<IEnumerable<VehicleBrandDto>> Handle(GetVehicleBrandsQuery request, CancellationToken cancellationToken = default)
    {
        var entities = await repository.GetAllAsync(cancellationToken);
        if (entities.Any())
        {
            return entities.Adapt<IEnumerable<VehicleBrandDto>>() ?? [];
        }
        return [];
    }
}
