using Mapster;
using MediatR;
using OneOf;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;

namespace workshopManager.Application.Queries.VehicleBrand;

public sealed record class GetVehicleBrandQuery : VehicleBrandDto, IRequest<OneOf<VehicleBrandDto, NotFoundException>>
{
    public GetVehicleBrandQuery(Guid id)
    {
        Id = id;
    }
}

public sealed class GetVehicleBrandQueryHandler(IVehicleBrandRepository repository) : IRequestHandler<GetVehicleBrandQuery, OneOf<VehicleBrandDto, NotFoundException>>
{
    public async Task<OneOf<VehicleBrandDto, NotFoundException>> Handle(GetVehicleBrandQuery request, CancellationToken cancellationToken = default)
    {
        var element = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (element is not null)
        {
            return element.Adapt<VehicleBrandDto>();
        }
        return new NotFoundException(request.Name);
    }
}
