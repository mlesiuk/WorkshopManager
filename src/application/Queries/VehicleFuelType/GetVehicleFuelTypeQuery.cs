using Mapster;
using MediatR;
using OneOf;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;

namespace workshopManager.Application.Queries.VehicleFuelType;

public sealed record class GetVehicleFuelTypeQuery : VehicleFuelTypeDto, IRequest<OneOf<VehicleFuelTypeDto, NotFoundException>>;

public sealed class GetVehicleFuelTypeQueryHandler 
    : IRequestHandler<GetVehicleFuelTypeQuery, OneOf<VehicleFuelTypeDto, NotFoundException>>
{
    private readonly IVehicleFuelTypeRepository _vehicleFuelTypeRepository;

    public GetVehicleFuelTypeQueryHandler(IVehicleFuelTypeRepository vehicleFuelTypeRepository)
    {
        _vehicleFuelTypeRepository = vehicleFuelTypeRepository;
    }

    public async Task<OneOf<VehicleFuelTypeDto, NotFoundException>> Handle(GetVehicleFuelTypeQuery request, CancellationToken cancellationToken)
    {
        var entity = await _vehicleFuelTypeRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
        {
            return new NotFoundException($"Fuel type with ID {request.Id} not found.");
        }

        return entity.Adapt<VehicleFuelTypeDto>();
    }
}
