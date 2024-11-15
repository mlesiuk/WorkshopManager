using Mapster;
using MediatR;
using OneOf;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;

namespace workshopManager.Application.Queries.VehicleBodyType;

public record GetVehicleBodyTypeByIdQuery : VehicleBodyTypeDto, IRequest<OneOf<VehicleBodyTypeDto, NotFoundException>>;

public sealed class GetVehicleBodyTypeByIdQueryHandler 
    : IRequestHandler<GetVehicleBodyTypeByIdQuery, OneOf<VehicleBodyTypeDto, NotFoundException>>
{
    private readonly IVehicleBodyTypeRepository _vehicleBodyTypeRepository;

    public GetVehicleBodyTypeByIdQueryHandler(IVehicleBodyTypeRepository vehicleBodyTypeRepository)
    {
        _vehicleBodyTypeRepository = vehicleBodyTypeRepository;
    }

    public async Task<OneOf<VehicleBodyTypeDto, NotFoundException>> Handle(GetVehicleBodyTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _vehicleBodyTypeRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
        {
            return new NotFoundException($"Body type with ID {request.Id} not found.");
        }

        return entity.Adapt<VehicleBodyTypeDto>();
    }
}
