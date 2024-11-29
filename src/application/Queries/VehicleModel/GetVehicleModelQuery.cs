using Mapster;
using MediatR;
using OneOf;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;
using workshopManager.Domain.Abstractions.Interfaces;

namespace workshopManager.Application.Queries.VehicleModel;

public sealed record class GetVehicleModelQuery(Guid Id) : IRequest<OneOf<VehicleModelDto, NotFoundException>>;

public sealed class GetVehicleModelByIdQueryHandler : IRequestHandler<GetVehicleModelQuery, OneOf<VehicleModelDto, NotFoundException>>
{
    private readonly IVehicleModelRepository _vehicleModelRepository;

    public GetVehicleModelByIdQueryHandler(IVehicleModelRepository vehicleModelRepository)
    {
        _vehicleModelRepository = vehicleModelRepository;
    }

    public async Task<OneOf<VehicleModelDto, NotFoundException>> Handle(GetVehicleModelQuery request, CancellationToken cancellationToken)
    {
        var entity = await _vehicleModelRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
        {
            return new NotFoundException($"VehicleModel with ID {request.Id} not found.");
        }

        return entity.Adapt<VehicleModelDto>();
    }
}
