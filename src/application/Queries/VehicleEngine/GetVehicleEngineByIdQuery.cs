using Mapster;
using MediatR;
using OneOf;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;

namespace workshopManager.Application.Queries.VehicleEngine;

public record GetVehicleEngineByIdQuery(Guid Id) : IRequest<OneOf<VehicleEngineDto, NotFoundException>>;

public sealed class GetVehicleEngineByIdQueryHandler : IRequestHandler<GetVehicleEngineByIdQuery, OneOf<VehicleEngineDto, NotFoundException>>
{
    private readonly IVehicleEngineRepository _vehicleEngineRepository;

    public GetVehicleEngineByIdQueryHandler(IVehicleEngineRepository vehicleEngineRepository)
    {
        _vehicleEngineRepository = vehicleEngineRepository;
    }

    public async Task<OneOf<VehicleEngineDto, NotFoundException>> Handle(GetVehicleEngineByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _vehicleEngineRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
        {
            return new NotFoundException($"VehicleEngine with ID {request.Id} not found.");
        }

        return entity.Adapt<VehicleEngineDto>();
    }
}
