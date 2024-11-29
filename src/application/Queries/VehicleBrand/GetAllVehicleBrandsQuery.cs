using Mapster;
using MediatR;
using workshopManager.Application.Dtos;
using workshopManager.Domain.Abstractions.Interfaces;

namespace workshopManager.Application.Queries.VehicleBrand;

public sealed record class GetAllVehicleBrandsQuery : VehicleBrandDto, IRequest<IEnumerable<VehicleBrandDto>> { }

public sealed class GetAllVehicleBrandsQueryHandler(IVehicleBrandRepository repository) : IRequestHandler<GetAllVehicleBrandsQuery, IEnumerable<VehicleBrandDto>>
{
    public async Task<IEnumerable<VehicleBrandDto>> Handle(GetAllVehicleBrandsQuery request, CancellationToken cancellationToken = default)
    {
        var entities = await repository.GetAllAsync(cancellationToken);
        if (entities.Any())
        {
            return entities.Adapt<IEnumerable<VehicleBrandDto>>() ?? [];
        }
        return [];
    }
}
