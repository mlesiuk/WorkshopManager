using Mapster;
using MediatR;
using OneOf;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;
using workshopManager.Domain.Abstractions.Interfaces;

namespace workshopManager.Application.Queries.VehicleGearbox;

public sealed record class GetVehicleGearboxQuery : VehicleGearboxDto, IRequest<OneOf<VehicleGearboxDto, NotFoundException>>;

public sealed class GetVehicleGearboxQueryHandler
    : IRequestHandler<GetVehicleGearboxQuery, OneOf<VehicleGearboxDto, NotFoundException>>
{
    private readonly IVehicleGearboxRepository _vehicleGearboxRepository;

    public GetVehicleGearboxQueryHandler(IVehicleGearboxRepository vehicleGearboxRepository)
    {
        _vehicleGearboxRepository = vehicleGearboxRepository;
    }

    public async Task<OneOf<VehicleGearboxDto, NotFoundException>> Handle(GetVehicleGearboxQuery request, CancellationToken cancellationToken)
    {
        var entity = await _vehicleGearboxRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
        {
            return new NotFoundException($"Fuel type with ID {request.Id} not found.");
        }

        return entity.Adapt<VehicleGearboxDto>();
    }
}
