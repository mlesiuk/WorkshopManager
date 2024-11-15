using MediatR;
using OneOf;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Application.Exceptions;

namespace workshopManager.Application.Commands.VehicleEngine;

public record DeleteVehicleEngineCommand(Guid Id) : IRequest<OneOf<bool, NotFoundException>>;

public sealed class DeleteVehicleEngineCommandHandler
    : IRequestHandler<DeleteVehicleEngineCommand, OneOf<bool, NotFoundException>>
{
    private readonly IVehicleEngineRepository _vehicleEngineRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVehicleEngineCommandHandler(
        IVehicleEngineRepository vehicleEngineRepository,
        IUnitOfWork unitOfWork)
    {
        _vehicleEngineRepository = vehicleEngineRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OneOf<bool, NotFoundException>> Handle(DeleteVehicleEngineCommand request, CancellationToken cancellationToken)
    {
        var entity = await _vehicleEngineRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
        {
            return new NotFoundException($"VehicleEngine with ID {request.Id} not found.");
        }

        await _vehicleEngineRepository.RemoveAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
