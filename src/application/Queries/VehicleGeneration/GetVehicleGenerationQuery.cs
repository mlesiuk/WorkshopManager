using Mapster;
using MediatR;
using OneOf;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;

namespace workshopManager.Application.Queries.VehicleGeneration;

public sealed record class GetVehicleGenerationQuery(Guid Id) : IRequest<OneOf<VehicleGenerationDto, NotFoundException>>;

public sealed class GetVehicleGenerationByIdQueryHandler : IRequestHandler<GetVehicleGenerationQuery, OneOf<VehicleGenerationDto, NotFoundException>>
{
    private readonly IVehicleGenerationRepository _vehicleGenerationRepository;

    public GetVehicleGenerationByIdQueryHandler(IVehicleGenerationRepository vehicleGenerationRepository)
    {
        _vehicleGenerationRepository = vehicleGenerationRepository;
    }

    public async Task<OneOf<VehicleGenerationDto, NotFoundException>> Handle(GetVehicleGenerationQuery request, CancellationToken cancellationToken)
    {
        var entity = await _vehicleGenerationRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
        {
            return new NotFoundException($"VehicleGeneration with ID {request.Id} not found.");
        }

        return entity.Adapt<VehicleGenerationDto>();
    }
}
