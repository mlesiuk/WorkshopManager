using Mapster;
using MediatR;
using OneOf;
using OneOf.Types;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Application.Dtos;
using VehicleBrandEntity = workshopManager.Domain.Entities.VehicleBrand;

namespace workshopManager.Application.Queries.VehicleBrand;

public sealed class GetVehicleBrandQuery : IRequest<OneOf<VehicleBrandDto, NotFound>>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public GetVehicleBrandQuery(Guid id)
    {
        Id = id;
    }

    public GetVehicleBrandQuery(VehicleBrandEntity vehicleBrand)
    {
        Id = vehicleBrand.Id;
        Name = vehicleBrand.Name;
    }
}

public sealed class GetVehicleBrandQueryHandler : IRequestHandler<GetVehicleBrandQuery, OneOf<VehicleBrandDto, NotFound>>
{
    private readonly IVehicleBrandRepository _repository;

    public GetVehicleBrandQueryHandler(IVehicleBrandRepository repository)
    {
        _repository = repository;
    }

    public async Task<OneOf<VehicleBrandDto, NotFound>> Handle(GetVehicleBrandQuery request, CancellationToken cancellationToken = default)
    {
        var element = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (element is not null)
        {
            return element.Adapt<VehicleBrandDto>();
        }
        return new NotFound();
    }
}

